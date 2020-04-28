using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WEB2Project.Dtos;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public AuthController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
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

        [HttpGet("getUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://pusgs.eu.auth0.com/api/v2/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStreamAsync();

            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(toReturn))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                var result = serializer.Deserialize(jsonTextReader);
                return Ok(result);
            }
        }

        [HttpPost("getUserByEmail")]
        public async Task<UserFromServer> GetUserByEmail([FromBody] JObject data)
        {
            var token = GetAuthorizationToken();

            string email = data["email"].ToString();

            string requestUri = "https://pusgs.eu.auth0.com/api/v2/users-by-email?email=" + email;
            requestUri.Replace("@", "%40");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Add("Authorization", "Bearer " + token);
        
            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserFromServer> user = JsonConvert.DeserializeObject<List<UserFromServer>>(toReturn);

            return user.First();
        }
     
        [HttpPatch("updateUser")]
        public async Task<IActionResult> UpdateUser(UserFromSPA userFromSpa)
        {
            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pusgs.eu.auth0.com/api/v2/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            UserToCreate userToCreate = new UserToCreate();

            userToCreate.email = userFromSpa.Email;
            userToCreate.email_verified = false;
            userToCreate.given_name = userFromSpa.FirstName;
            userToCreate.family_name = userFromSpa.LastName;
            userToCreate.name = userFromSpa.FirstName + " " + userFromSpa.LastName;
            userToCreate.connection = "Username-Password-Authentication";
            userToCreate.password = userFromSpa.Password;

            var client = _clientFactory.CreateClient();

            var userInJson = JsonConvert.SerializeObject(userToCreate);

            var stringContent = new StringContent(userInJson, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStreamAsync();

            var serializer = new JsonSerializer();

            using (var sr = new StreamReader(toReturn))
            using (var jsonTextReader = new JsonTextReader(sr))
            {
                var result = serializer.Deserialize(jsonTextReader);
                return Ok(result);
            }
        }

        [HttpPost("createAdminUser")]
        public async Task<IActionResult> CreateAdminUser(UserFromSPA userFromSpa)
        {
            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pusgs.eu.auth0.com/api/v2/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            UserToCreate userToCreate = new UserToCreate();

            userToCreate.email = userFromSpa.Email;
            userToCreate.email_verified = false;
            userToCreate.given_name = userFromSpa.FirstName;
            userToCreate.family_name = userFromSpa.LastName;
            userToCreate.name = userFromSpa.FirstName + " " + userFromSpa.LastName;
            userToCreate.connection = "Username-Password-Authentication";
            userToCreate.password = userFromSpa.Password;

            var client = _clientFactory.CreateClient();

            var userInJson = JsonConvert.SerializeObject(userToCreate);

            var stringContent = new StringContent(userInJson, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var response = await client.SendAsync(request);

            HttpStatusCode statusCode = response.StatusCode;

            if(statusCode == HttpStatusCode.Created)
            {
                var toReturn = await response.Content.ReadAsStringAsync();

                UserFromServer user = JsonConvert.DeserializeObject<UserFromServer>(toReturn);

                string roleId = await CreateRole(userFromSpa.CompanyId.ToString());
                HttpStatusCode statusCode2 = await AssignRole(roleId, user.user_id);

                if (statusCode2 == HttpStatusCode.OK)
                    return Ok();
                else
                    throw new Exception("Error while creating new admin user");
            }
            else
                throw new Exception("Error while creating new admin user");

        }

        public async Task<string> CreateRole(string companyId)
        {
            RoleToCreate role = new RoleToCreate();
            role.description = "Manage Company With Id:" + companyId;
            role.name = "managerNo" + companyId;

            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pusgs.eu.auth0.com/api/v2/roles");

            request.Headers.Add("Authorization", "Bearer " + token);
            var roleInJson = JsonConvert.SerializeObject(role);

            var stringContent = new StringContent(roleInJson, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            UserRole roleToReturn = JsonConvert.DeserializeObject<UserRole>(toReturn);

            return roleToReturn.id;
        }

        [HttpPost("getUserRoles")]
        public async Task<IActionResult> GetUserRoles([FromBody] JObject data)
        {
            var user = await GetUserByEmail(data);
            string uriString = "https://pusgs.eu.auth0.com/api/v2/users/" + user.user_id + "/roles";
            uriString = uriString.Replace("|", "%7C");

            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Get, uriString);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserRole> roles = JsonConvert.DeserializeObject<List<UserRole>>(toReturn);

            return Ok(roles);
        }


        public async Task<HttpStatusCode> AssignRole(string roleId, string userId)
        {
            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://pusgs.eu.auth0.com/api/v2/roles/" + roleId + "/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            string body = "{\"users\": [\"" + userId + "\"]}";
         
            var stringContent = new StringContent(body, Encoding.UTF8, "application/json");

            request.Content = stringContent;

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = response.StatusCode;

            return toReturn;
        }

    }
}