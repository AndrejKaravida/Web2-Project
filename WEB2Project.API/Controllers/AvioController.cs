using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Models;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvioController : ControllerBase
    {
        private readonly IFlightsRepository _repo;
        public AvioController(IFlightsRepository repo)
        {
            _repo = repo;

        }

        [HttpGet("getCompany/{id}", Name = "GetAvioCompany")]

        public async Task<IActionResult> GetAvioCompany(int id)
        {
            var company = await _repo.GetCompany(id);

            return Ok(company);
        }

        [HttpGet("aircompanies")]
        public IActionResult GetAllCompanies()
        {
            var companies = _repo.GetAllCompanies();

            return Ok(companies);
        }

        [HttpGet("destinations")]
        public IActionResult GetAllDestinations()
        {
            var destinations = _repo.GetAllDestinations();

            return Ok(destinations);
        }

    }
}