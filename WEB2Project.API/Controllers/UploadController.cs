using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEB2Project.Data;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IImageHandler _imageHandler;
        private readonly IRentACarRepository _repo;
        private readonly IMapper _mapper;

        public UploadController(IImageHandler imageHandler, IRentACarRepository repo, IMapper mapper)
        {
            _imageHandler = imageHandler;
            _repo = repo;
            _mapper = mapper;
        }

        [HttpPost("{vehicleId}")]
        public async Task<IActionResult> UploadImage(int vehicleId, IFormFile file)
        {
            var vehicle = _repo.GetVehicle(vehicleId);
            var image_location = await _imageHandler.UploadImage(file);
            var objectResult = image_location as ObjectResult;
            var value = objectResult.Value;

            vehicle.Photo = "http://localhost:5000" + value.ToString();

            if (await _repo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Could not add the photo");

        }


    }
}