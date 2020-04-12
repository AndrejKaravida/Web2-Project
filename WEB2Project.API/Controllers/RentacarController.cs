using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Dtos;
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

        [HttpPost("addNewDestination/{companyId}")]
        public async Task<IActionResult> AddNewDestination(int companyId, DestinationToAdd destination)
        {
          
            var companyFromRepo = await _repo.GetCompany(companyId);

            Destination newDestination = new Destination()
            {
                City = destination.City,
                Country = destination.Country,
                MapString = destination.MapString
            };

            _repo.Add(newDestination);
            await _repo.SaveAll();

            companyFromRepo.Destinations.Add(newDestination);

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Editing company failed on save!");
        }

        [HttpPost]
        public async Task<IActionResult> EditCompany(RentACarCompany company)
        {
            var companyFromRepo = await _repo.GetCompany(company.Id);

            companyFromRepo.Name = company.Name;
            companyFromRepo.PromoDescription = company.PromoDescription;
            companyFromRepo.MonthRentalDiscount = company.MonthRentalDiscount;
            companyFromRepo.WeekRentalDiscount = company.WeekRentalDiscount;

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Editing company failed on save!");
        }

        [HttpPost("addCompany")]
        public async Task<IActionResult> MakeNewCompany(CompanyToMake companyToMake)
        {
            Destination destination = new Destination();
            destination.City = companyToMake.City;
            destination.Country = companyToMake.Country;
            destination.MapString = companyToMake.MapString;

            _repo.Add(destination);
            await _repo.SaveAll();

            RentACarCompany company = new RentACarCompany()
            {
                Name = companyToMake.Name,
                PromoDescription = "Temporary promo description",
                AverageGrade = 0,
                Destinations = new List<Destination>(),
                WeekRentalDiscount = 0,
                MonthRentalDiscount = 0
            };

            company.Destinations.Add(destination);

            _repo.Add(company);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetRentACarCompany", new { id = company.Id }, company);
            else
                throw new Exception("Saving vehicle failed on save!");
        }

        [HttpGet("carcompanies")]
        public IActionResult GetRentACarCompaniesNoPaging()
        {
            var companies = _repo.GetAllCompanies();

            return Ok(companies);
        }

        [HttpGet("getVehicles/{companyId}")]
        public async Task<IActionResult> GetVehiclesForCompany(int companyId, [FromQuery]VehicleParams companyParams)
        {
            var vehicles = await _repo.GetVehiclesForCompany(companyId, companyParams);

            Response.AddPagination(vehicles.CurrentPage, vehicles.PageSize,
             vehicles.TotalCount, vehicles.TotalPages);

            return Ok(vehicles);
        }

        [HttpGet("getDiscountedVehicles/{companyId}")]
        public IActionResult GetDiscountedVehicles(int companyId)
        {
            var discountedVehicles = _repo.GetDiscountedVehicles(companyId);

            return Ok(discountedVehicles);
        }

        [HttpGet("getVehicle/{id}", Name = "GetVehicle")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _repo.GetVehicle(id);

            return Ok(vehicle);
        }

        [HttpPost("getIncomes/{companyid}", Name = "GetCompanyIncomes")]
        public IActionResult GetCompanyIncomes(int companyid, IncomeData data)
        {
            var incomes = _repo.GetCompanyIncomes(companyid);
            var startingDate = data.StartingDate.Date;
            var finalDate = data.FinalDate.Date;

            incomes = incomes.Where(x => x.Date.Date >= startingDate && x.Date.Date <= finalDate).ToList();

            List<DateTime> dates = new List<DateTime>();

            foreach (var income in incomes)
            {
                if (!dates.Contains(income.Date.Date))
                {
                    dates.Add(income.Date.Date);
                }
            }

            Dictionary<int, double> keyValuePairs = new Dictionary<int, double>();   

            for (int i = 0; i < dates.Count; i++)
            {
                keyValuePairs.Add(i, 0);                   
            }


            for (int i = 0; i < dates.Count; i++)
            {
                foreach(var income in incomes)
                {
                    if(income.Date.Date == dates[i])
                    {
                        keyValuePairs[i] += income.Value;
                    }
                }
            }

            List<double> incomeValues = new List<double>();
            List<string> incomeDates = new List<string>();

            foreach (var kvp in keyValuePairs)
            {
                incomeValues.Add(Math.Round(kvp.Value, 2));
                incomeDates.Add(dates[kvp.Key].ToShortDateString());
            }

            IncomeStatsToReturn incomeStatsToReturn = new IncomeStatsToReturn();
            incomeStatsToReturn.values = incomeValues.ToArray();
            incomeStatsToReturn.dates = incomeDates.ToArray();

            return Ok(incomeStatsToReturn);
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

        [HttpPost("changeHeadOffice/{companyId}")]

        public async Task<IActionResult> ChangeHeadOffice (int companyId, [FromBody]JObject data)
        {
            var company = await _repo.GetCompany(companyId);
            var headOffice = data["headOffice"].ToString();

            if (company.HeadOffice.City == headOffice)
                return NoContent();

            var destination = company.Destinations.Where(d => d.City == headOffice).FirstOrDefault();
            company.HeadOffice = destination;

            await _repo.SaveAll(); 

            return Ok();
        }

        [HttpPost("removeDestination/{companyId}")]
        public async Task<IActionResult> RemoveDestination(int companyId, [FromBody]JObject data)
        {
            var company = await _repo.GetCompany(companyId);
            var location = data["location"].ToString();

            if (company.HeadOffice.City == location)
                return NoContent();

            var destination = company.Destinations.Where(d => d.City == location).FirstOrDefault();

            company.Destinations.Remove(destination);

            await _repo.SaveAll();

            return Ok();
        }
    }
}