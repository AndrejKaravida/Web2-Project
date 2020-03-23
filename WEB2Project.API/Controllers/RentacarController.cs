using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Models;

namespace WEB2Project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RentacarController : ControllerBase
    {
        private readonly IRentACarRepository _repo;

        public RentacarController(IRentACarRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("{id}", Name = "GetRentACarCompany")]
        public async Task<IActionResult> GetRentACarCompany(int id)
        {
            var company = await _repo.GetCompany(id);

            return Ok(company);
        }

        [HttpGet]
        public IActionResult GetRentACarCompanies()
        {
            var companies =  _repo.GetAllCompanies();

            return Ok(companies);
        }
    }
}