using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Helpers;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

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

        [HttpPost("rateVehicle/{vehicleId}")]
        public async Task<IActionResult> RateVehicle(int vehicleId, [FromBody]JObject data)
        {
            var vehicle = _repo.GetVehicle(vehicleId);

            int rating = Int32.Parse(data["rating"].ToString());

            Rating newRating = new Rating() {Value = rating };
            vehicle.Ratings.Add(newRating);

            var ratingsCount = vehicle.Ratings.Count;

            var totalRatings = 0;

            foreach(var r in vehicle.Ratings)
            {
                totalRatings += r.Value;
            }

            double averageRating = totalRatings / ratingsCount;

            vehicle.AverageGrade = Math.Round(averageRating, 2);

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Saving raing failed on save!");
        }

        [HttpPost("rateCompany/{companyId}")]
        public async Task<IActionResult> RateCompany(int companyId, [FromBody]JObject data)
        {
            var company = await _repo.GetCompany(companyId);

            int rating = Int32.Parse(data["rating"].ToString());

            Rating newRating = new Rating() { Value = rating };
            company.Ratings.Add(newRating);

            var ratingsCount = company.Ratings.Count;

            var totalRatings = 0;

            foreach (var r in company.Ratings)
            {
                totalRatings += r.Value;
            }

            double averageRating = totalRatings / ratingsCount;

            company.AverageGrade = Math.Round(averageRating, 2);

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Saving raing failed on save!");

        }
    }
}