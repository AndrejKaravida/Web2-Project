using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WEB2Project.Data;
using WEB2Project.Dtos;
using WEB2Project.Helpers;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {
        private readonly IImageHandler _imageHandler;
        private readonly IRentACarRepository _repo;
        private readonly IFlightsRepository _aviorepo;
        private readonly IHttpClientFactory _clientFactory;

        public UploadController(IImageHandler imageHandler, IRentACarRepository repo, IFlightsRepository aviorepo, IHttpClientFactory clientFactory)
        {
            _imageHandler = imageHandler;
            _repo = repo;
            _aviorepo = aviorepo;
            _clientFactory = clientFactory;
        }

        [HttpPost("{vehicleId}/{companyId}")]
        public async Task<IActionResult> UploadImage(int vehicleId, int companyId, IFormFile file)
        {
            var companyFromRepo = await _repo.GetCompany(companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != companyFromRepo.Admin.AuthId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var vehicle = _repo.GetVehicle(vehicleId);
            var image_location = await _imageHandler.UploadImage(file);
            var objectResult = image_location as ObjectResult;
            var value = objectResult.Value;

            vehicle.Photo = "http://localhost:5000/" + value.ToString();

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not add the photo");

        }

        [HttpPost("newCompany/{companyId}")]
        public async Task<IActionResult> UploadImageForCompany(int companyId, IFormFile file)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var company = _repo.GetCompany(companyId);
            var image_location = await _imageHandler.UploadImage(file);
            var objectResult = image_location as ObjectResult;
            var value = objectResult.Value;

            company.Result.Photo = "http://localhost:5000/" + value.ToString();

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not add the photo");

        }

        [HttpPost("newAvioCompany/{companyId}")]
        public async Task<IActionResult> UploadImageForAvioCompany(int companyId, IFormFile file)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var company = _aviorepo.GetCompany(companyId);
            var image_location = await _imageHandler.UploadImage(file);
            var objectResult = image_location as ObjectResult;
            var value = objectResult.Value;

            company.Photo = "http://localhost:5000/" + value.ToString();

            if (await _aviorepo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not add the photo");

        }

        public async Task<string> GetUserId(string email)
        {
            var token = GetAuthorizationToken();

            string requestUri = "https://pusgs.eu.auth0.com/api/v2/users-by-email?email=" + email;
            requestUri.Replace("@", "%40");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserFromServer> users = JsonConvert.DeserializeObject<List<UserFromServer>>(toReturn);

            UserFromServer user = users.First();

            return user.user_id;
        }

        public string GetAuthorizationToken()
        {
            var client = new RestClient("https://pusgs.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"i1ZqGVSnFuJOSsJxe00MhRp1UZ5CQDlw\",\"client_secret\":\"863wgBE7Yh0KG5TELRqCvoww926UD_5TftkBAY__F2LnSsh3nuB56OjAyI3PqolQ\",\"audience\":\"https://pusgs.eu.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic data = JObject.Parse(response.Content);

            return data.access_token;
        }
    }
}