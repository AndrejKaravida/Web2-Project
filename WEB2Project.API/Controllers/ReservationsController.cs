using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using WEB2Project.API.Models.AircompanyModels;
using WEB2Project.Data;
using WEB2Project.Dtos;
using WEB2Project.Helpers;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IRentACarRepository _repo;
        private readonly IFlightsRepository _repository;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public ReservationsController(IRentACarRepository repo, IFlightsRepository repository,
            IHttpClientFactory clientFactory, IMapper mapper)
        {
            _repo = repo;
            _repository = repository;
            _clientFactory = clientFactory;
            _mapper = mapper;
        }
        
        [HttpPost("flightreservation")]
        public async Task<IActionResult> MakeFlightReservation([FromBody]JObject data)
        {
            int flightId = Int32.Parse(data["flightId"].ToString());
            Flight flight = _repository.GetFlight(flightId);

            var depDate = data["departureTime"].ToString();
            var arrDate = data["arrivalTime"].ToString();

            depDate = depDate.Replace('-', '/');
            arrDate = arrDate.Replace('-', '/');

            DateTime dep = Convert.ToDateTime(depDate);
            DateTime arr = Convert.ToDateTime(arrDate);

            FlightReservation reservation = new FlightReservation()
            {
                UserAuthId = data["authId"].ToString(),
                DepartureDestination = data["departureDestination"].ToString(),
                CompanyId = Int32.Parse(data["companyId"].ToString()),
                CompanyName = data["companyName"].ToString(),
                CompanyPhoto = data["companyPhoto"].ToString(),
                ArrivalDestination = data["arrivalDestination"].ToString(),
                DepartureDate = dep,
                Flight = flight,
                ArrivalDate = arr, 
                Price = Double.Parse(data["price"].ToString()),
                TravelLength = Double.Parse(data["travelLength"].ToString()), 
                Status = "Active"
            };

            _repository.Add(reservation);
        
            await _repository.SaveAll();

            return Ok();
        }

        [HttpPost("carreservation")]
        public async Task<IActionResult> MakeCarReservation([FromBody]CarReservtion data)
        {
            Vehicle vehicle = _repo.GetVehicle(data.VehicleId);

            data.Startdate = data.Startdate.Replace('-', '/');
            data.Enddate = data.Enddate.Replace('-', '/');

            DateTime start = DateTime.ParseExact(data.Startdate, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(data.Enddate, "d/M/yyyy", CultureInfo.InvariantCulture);

            foreach(var r in vehicle.ReservedDates)
            {
                if(r.Date >= start.Date && r.Date <= end.Date)
                {
                    return BadRequest("Concurency error");
                }
            }

            Reservation reservation = new Reservation()
            {
                UserAuthId = data.AuthId,
                Vehicle = vehicle,
                StartDate = start,
                EndDate = end,
                CompanyName = data.Companyname,
                StartingLocation = data.StartingLocation,
                ReturningLocation = data.ReturningLocation,
                CompanyId = data.CompanyId,
                NumberOfDays = data.Totaldays,
                TotalPrice = data.Totalprice,
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
            
            var companyFromRepo = await _repo.GetCompany(data.CompanyId);
            Income newIncome = new Income() { Date = DateTime.Now, Value = reservation.TotalPrice };

            if (companyFromRepo.Incomes == null)
                companyFromRepo.Incomes = new List<Income>();
            companyFromRepo.Incomes.Add(newIncome);

            if (await _repo.SaveAll())
                return NoContent();

           return BadRequest("Saving reservation failed on save");
        }

        [HttpGet("{authId}")]
        public async Task<IActionResult> GetCarReservationsForUser(string authId)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != authId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var reservations = await _repo.GetCarReservationsForUser(authId);

            foreach (var res in reservations)
            {
                if(res.StartDate.Date >= DateTime.Now.Date)
                {
                    res.DaysLeft = (res.EndDate.Date - res.StartDate.Date).TotalDays;
                }
                else
                {
                    res.DaysLeft = (res.EndDate.Date - DateTime.Now.Date).TotalDays;
                    if (res.DaysLeft < 0)
                        res.DaysLeft = 0;
                }
            }

            var reservationsToReturn = _mapper.Map<List<ReservationToReturn>>(reservations); 
            return Ok(reservationsToReturn);
        }

        [HttpGet("flightReservations/{authId}")]
        public IActionResult GetFlightReservationsForUser(string authId)
        {

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != authId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var reservations = _repository.GetFlightReservationsForUser(authId);

            return Ok(reservations);
        }

        public async Task<string> GetUserId(string email)
        {
            var token = GetAuthorizationToken();

            string requestUri = "https://pusgs.eu.auth0.com/api/v2/users-by-email?email=" + email;
            requestUri.Replace("@", "%40");

            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserFromServer> users = JsonConvert.DeserializeObject<List<UserFromServer>>(toReturn);

            UserFromServer user = users.First();

            return user.user_id;
        }

        public string GetAuthorizationToken()
        {
            var client = new RestClient("https://pusgs.eu.auth0.com/oauth/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", "{\"client_id\":\"i1ZqGVSnFuJOSsJxe00MhRp1UZ5CQDlw\",\"client_secret\":\"863wgBE7Yh0KG5TELRqCvoww926UD_5TftkBAY__F2LnSsh3nuB56OjAyI3PqolQ\",\"audience\":\"https://pusgs.eu.auth0.com/api/v2/\",\"grant_type\":\"client_credentials\"}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            dynamic data = JObject.Parse(response.Content);

            return data.access_token;
        }
    }
}