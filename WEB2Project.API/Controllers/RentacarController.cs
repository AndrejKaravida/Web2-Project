using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        private readonly IMapper _mapper;

        public RentacarController(IRentACarRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
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
        public async Task<IActionResult> GetRentACarCompanies([FromQuery]VehicleParams companyParams)
        {
            var companies = await _repo.GetAllCompanies(companyParams);

            Response.AddPagination(companies.CurrentPage, companies.PageSize, companies.TotalCount, companies.TotalPages);

            return Ok(companies);
        }

        [HttpGet("getVehicles/{companyId}")]
        public IActionResult GetVehiclesForCompany(int companyId, [FromQuery]VehicleParams companyParams)
        {
            var vehicles = _repo.GetVehiclesForCompany(companyId, companyParams);

            return Ok(vehicles);
        }

        [HttpGet("getVehiclesNoParams/{companyId}")]
        public IActionResult GetVehiclesForCompanyNoParams(int companyId)
        {
            var vehicles = _repo.GetVehiclesForCompanyWithoutParams(companyId);

            return Ok(vehicles);
        }
    }
}