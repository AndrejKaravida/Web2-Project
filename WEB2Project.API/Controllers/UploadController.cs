using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB2Project.Data;

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

        public UploadController(IImageHandler imageHandler, IRentACarRepository repo, IFlightsRepository aviorepo)
        {
            _imageHandler = imageHandler;
            _repo = repo;
            _aviorepo = aviorepo;
        }

        [HttpPost("{vehicleId}")]
        public async Task<IActionResult> UploadImage(int vehicleId, IFormFile file)
        {
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
    }
}