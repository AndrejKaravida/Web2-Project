using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WEB2Project.Data;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IRentACarRepository _repo;

        public ReservationsController(IRentACarRepository repo)
        {
            _repo = repo;
        }

        [HttpPost]
        public async Task<IActionResult> MakeCarReservation([FromBody]JObject data)
        {
            int vehicle_id = Int32.Parse(data["vehicleId"].ToString());
            int company_id = Int32.Parse(data["companyid"].ToString());

            Vehicle vehicle = _repo.GetVehicle(vehicle_id);

            var startDate = data["startdate"].ToString();
            var endDate = data["enddate"].ToString();

            startDate = startDate.Replace('-', '/');
            endDate = endDate.Replace('-', '/');

            DateTime start = DateTime.ParseExact(startDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "d/M/yyyy", CultureInfo.InvariantCulture);

            int days = Int32.Parse(data["totaldays"].ToString());
            double price = Double.Parse(data["totalprice"].ToString());

            vehicle.CurrentDestination = data["returningLocation"].ToString();

            Reservation reservation = new Reservation()
            {
                UserName = data["username"].ToString(),
                Vehicle = vehicle, 
                StartDate = start, 
                EndDate = end,
                CompanyName = data["companyname"].ToString(),
                CompanyId = company_id,
                NumberOfDays = days,
                TotalPrice = price, 
                Status = "Active"
            };

            _repo.Add(reservation);

            if(vehicle.ReservedDates == null)
            {
                vehicle.ReservedDates = new List<ReservedDate>();
            }

            for(var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                ReservedDate date = new ReservedDate { Date = dt };
                vehicle.ReservedDates.Add(date);
            }
            
            var companyFromRepo = await _repo.GetCompany(company_id);
            Income newIncome = new Income() { Date = DateTime.Now, Value = reservation.TotalPrice };

            if (companyFromRepo.Incomes == null)
                companyFromRepo.Incomes = new List<Income>();
            companyFromRepo.Incomes.Add(newIncome);

            if (await _repo.SaveAll())
                return NoContent();

            throw new Exception("Saving reservation failed on save");
        }

        [HttpGet("{username}")]
        public IActionResult GetCarReservationsForUser(string username)
        {
            var reservations = _repo.GetCarReservationsForUser(username);

            foreach (var res in reservations)
            {
               res.DaysLeft = (res.EndDate.Date - DateTime.Now.Date).TotalDays;
                if (res.DaysLeft < 0)
                    res.DaysLeft = 0;
            }

            return Ok(reservations);
        }
    }
}