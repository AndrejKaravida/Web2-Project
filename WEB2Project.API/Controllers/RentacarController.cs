using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
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

        [HttpGet("companies")]
        public async Task<IActionResult> GetRentACarCompanies([FromQuery]VehicleParams companyParams)
        {
            var companies = await _repo.GetAllCompanies(companyParams);

            Response.AddPagination(companies.CurrentPage, companies.PageSize, companies.TotalCount, companies.TotalPages);

            return Ok(companies);
        }

        [HttpGet("carcompanies")]
        public IActionResult GetRentACarCompaniesNoPaging()
        {
            var companies = _repo.GetAllCompaniesNoPaging();

        
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

        [HttpGet("getVehicle/{id}", Name = "GetVehicle")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _repo.GetVehicle(id);

            return Ok(vehicle);
        }

        [HttpGet("getIncomes/{companyid}", Name = "GetCompanyIncomes")]
        public IActionResult GetCompanyIncomes(int companyid)
        {
            var incomes = _repo.GetCompanyIncomes(companyid);

            var dateToday = DateTime.Now.Date;

            double incomeToday = 0;
            double incomeThisWeek = 0;
            double incomeThisMonth = 0;

            foreach (var income in incomes)
            {
                if (income.Date.Date == dateToday)
                {
                    incomeToday += income.Value;
                }
                if (income.Date.Date <= dateToday.Date.AddDays(6))
                {
                    incomeThisWeek += income.Value;
                }
                if (income.Date.Date <= dateToday.Date.AddDays(30))
                {
                    incomeThisMonth += income.Value;
                }
            }

            IncomeStatsToReturn stats = new IncomeStatsToReturn()
            {
                IncomeToday = Int32.Parse(incomeToday.ToString()),
                IncomeThisWeek = Int32.Parse(incomeThisWeek.ToString()),
                IncomeThisMonth = Int32.Parse(incomeThisMonth.ToString())
            };

            return Ok(stats);
        }

        [HttpGet("getReservations/{companyid}", Name = "GetCompanyReservations")]
        public IActionResult GetCompanyReservartions(int companyid)
        {
            var reservations = _repo.GetCompanyReservations(companyid);

            var dateToday = DateTime.Now.Date;

            int reservationsToday = 0;
            int reservationsThisWeek = 0;
            int reservationsThisMonth = 0;

            foreach(var reservation in reservations)
            {
                if(reservation.StartDate.Date == dateToday)
                {
                    reservationsToday++;
                }
                if(reservation.StartDate.Date <= dateToday.Date.AddDays(6))
                {
                    reservationsThisWeek++;
                }
                if (reservation.StartDate.Date <= dateToday.Date.AddDays(30))
                {
                    reservationsThisMonth++;
                }
            }

            ReservationStatsToReturn stats = new ReservationStatsToReturn()
            {
                ReservationsToday = reservationsToday,
                ReservationsThisWeek = reservationsThisWeek,
                ReservationsThisMonth = reservationsThisMonth
            };

            return Ok(stats);
        }

        [HttpPost("newVehicle/{companyId}")]
        public async Task<IActionResult> MakeNewVehicle (int companyId, Vehicle vehicleFromBody)
        {
            Vehicle vehicle = new Vehicle()
            {
                Manufacturer = vehicleFromBody.Manufacturer,
                Model = vehicleFromBody.Model,
                AverageGrade = 0,
                Ratings = new List<VehicleRating>(),
                Doors = vehicleFromBody.Doors,
                Seats = vehicleFromBody.Seats,
                Price = vehicleFromBody.Price,
                IsDeleted = false,
                Photo = "",
                Type = vehicleFromBody.Type
            };

            _repo.Add(vehicle);

            var companyFromRepo = await _repo.GetCompany(companyId);
            companyFromRepo.Vehicles.Add(vehicle);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetVehicle", new { id = vehicle.Id }, vehicle);
            else
                throw new Exception("Saving vehicle failed on save!");
        }

        [HttpPost("editVehicle/{vehicleId}")]
        public async Task<IActionResult> EditVehicle(int vehicleId, Vehicle vehicleFromBody)
        {
            var vehicle = _repo.GetVehicle(vehicleId);
            vehicle.Manufacturer = vehicleFromBody.Manufacturer;
            vehicle.Model = vehicleFromBody.Model;
            vehicle.Doors = vehicleFromBody.Doors;
            vehicle.Seats = vehicleFromBody.Seats;
            vehicle.Price = vehicleFromBody.Price;
            vehicle.Type = vehicleFromBody.Type;

            if (await _repo.SaveAll())
                return NoContent();
            else
                throw new Exception("Saving vehicle failed on save!");
        }

        [HttpGet("deleteVehicle/{vehicleId}")]
        public async Task<IActionResult> DeleteVehicle(int vehicleId)
        {
            var vehicle = _repo.GetVehicle(vehicleId);
            vehicle.IsDeleted = true;

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Deleting vehicle failed on save!");
        }

        [HttpPost("rateVehicle/{vehicleId}")]
        public async Task<IActionResult> RateVehicle(int vehicleId, [FromBody]JObject data)
        {
            var vehicle = _repo.GetVehicle(vehicleId);

            int rating = Int32.Parse(data["rating"].ToString());

            VehicleRating newRating = new VehicleRating() {Value = rating };
            vehicle.Ratings.Add(newRating);

            double ratingsCount = vehicle.Ratings.Count;

            double totalRatings = 0;

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

            CompanyRating newRating = new CompanyRating() { Value = rating };
            company.Ratings.Add(newRating);

            double ratingsCount = company.Ratings.Count;

            double totalRatings = 0;

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