using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public class ReservationsController : ControllerBase
    {
        private readonly IRentACarRepository _repo;
        private readonly IFlightsRepository _repository;
        private readonly IHttpClientFactory _clientFactory;

        public ReservationsController(IRentACarRepository repo, IFlightsRepository repository, IHttpClientFactory clientFactory)
        {
            _repo = repo;
            _repository = repository;
            _clientFactory = clientFactory;
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

            var startDate = data["startdate"].ToString();
            var endDate = data["enddate"].ToString();

            startDate = startDate.Replace('-', '/');
            endDate = endDate.Replace('-', '/');

            DateTime start = DateTime.ParseExact(startDate, "d/M/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(endDate, "d/M/yyyy", CultureInfo.InvariantCulture);

            int days = Int32.Parse(data["totaldays"].ToString());
            double price = Double.Parse(data["totalprice"].ToString());

            Reservation reservation = new Reservation()
            {
                UserName = data["username"].ToString(),
                Vehicle = vehicle,
                StartDate = start,
                EndDate = end,
                CompanyName = data["companyname"].ToString(),
                StartingLocation = data["startingLocation"].ToString(),
                ReturningLocation = data["returningLocation"].ToString(),
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
        public async Task<IActionResult> GetCarReservationsForUser(string username)
        {
            var userId = await GetUserId(username);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != userId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var reservations = _repo.GetCarReservationsForUser(username);

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