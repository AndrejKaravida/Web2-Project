using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using WEB2Project.API.Models.AircompanyModels;
using WEB2Project.Data;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IRentACarRepository _repo;
        private readonly IFlightsRepository _repository;

        public ReservationsController(IRentACarRepository repo, IFlightsRepository repository)
        {
            _repo = repo;
            _repository = repository;
        }
        

        [HttpPost("flightreservation")]

        public async Task<IActionResult> MakeFlightReservation([FromBody]JObject data)
        { 
            var depDate = data["departureDate"].ToString();
            var arrDate = data["arrivalDate"].ToString();

            depDate = depDate.Replace('-', '/');
            arrDate = arrDate.Replace('-', '/');

            DateTime dep = DateTime.ParseExact(depDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime arr = DateTime.ParseExact(arrDate, "d/M/yyyy", CultureInfo.InvariantCulture);

            FlightReservation reservation = new FlightReservation()
            {
                Email = data["email"].ToString(),
                DepartureDestination = data["departureDestination"].ToString(),
                ArrivalDestination = data["arrivalDestination"].ToString(),
                DepartureDate = dep,
                ArrivalDate = arr, 
                Price = Double.Parse(data["price"].ToString()),
                TravelLength = Double.Parse(data["travelLength"].ToString()),
                Seats = data["seats"].ToString()
                
            };

            _repository.Add(reservation);
        
            await _repository.SaveAll();

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> MakeCarReservation([FromBody]JObject data)
        {
            int vehicle_id = Int32.Parse(data["vehicleId"].ToString());
            int company_id = Int32.Parse(data["companyid"].ToString());

            Vehicle vehicle = _repo.GetVehicle(vehicle_id);
            vehicle.IsReserved = true;

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