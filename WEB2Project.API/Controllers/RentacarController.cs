using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Helpers;
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

        [HttpPost]
        public async Task<IActionResult> EditCompany(RentACarCompany company)
        {
            var companyFromRepo = await _repo.GetCompany(company.Id);

            companyFromRepo.Name = company.Name;
            companyFromRepo.Address = company.Address;
            companyFromRepo.PromoDescription = company.PromoDescription;
            companyFromRepo.MonthRentalDiscount = company.MonthRentalDiscount;
            companyFromRepo.WeekRentalDiscount = company.WeekRentalDiscount;

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Editing company failed on save!");
        }

        [HttpGet]
        public IActionResult GetRentACarCompanies([FromQuery]CarCompanyParams companyParams)
        {
            var companies =  _repo.GetAllCompanies(companyParams);

            return Ok(companies);
        }
    }
}