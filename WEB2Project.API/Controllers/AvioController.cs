using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Dtos;
using WEB2Project.Helpers;
using WEB2Project.Models;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvioController : ControllerBase
    {
        private readonly IFlightsRepository _repo;
        private readonly IUsersRepository _userRepo;
        private readonly IHttpClientFactory _clientFactory;

        public AvioController(IFlightsRepository repo, IUsersRepository userRepo, IHttpClientFactory clientFactory)
        {
            _repo = repo;
            _userRepo = userRepo;
            _clientFactory = clientFactory;

        }

        [HttpGet("getCompany/{id}", Name = "GetAvioCompany")]
        public IActionResult GetAvioCompany(int id)
        {
            var company = _repo.GetCompany(id);

            return Ok(company);
        }

        [HttpPost("editHeadOffice/{companyId}")]
        //[Authorize]
        public async Task<IActionResult> EditHeadOffice(int companyId, [FromBody]JObject data)
        {
            var company = _repo.GetCompany(companyId);
            var headOffice = data["headOffice"].ToString();

            if (company.HeadOffice.City == headOffice)
                return NoContent();

            var destinations = _repo.GetAllDestinations();
            var destination = destinations.Where(x => x.City == headOffice).FirstOrDefault();
            company.HeadOffice = destination;

            await _repo.SaveAll();

            return Ok();
        }

        [HttpGet("aircompanies")]
        public async Task <IActionResult> GetAllCompanies()
        {
            var companies = _repo.GetAllCompanies();

            var company = companies.Where(x => x.Id == 1).FirstOrDefault();

            if (company.Admin == null)
            {
                await LoadAdmins();
            }

            return Ok(companies);
        }

        [HttpPost("editcompany/{copmanyId}")]
        //[Authorize]
        public async Task<IActionResult>EditCompany(int copmanyId, AirCompany companyToEdit)
        {
            var company =  _repo.GetCompany(copmanyId);
            company.Name = companyToEdit.Name;
            company.PromoDescription = companyToEdit.PromoDescription;

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Editing company failed on save!");
        }

        [HttpGet("destinations")]
        public IActionResult GetAllDestinations()
        {
            var destinations = _repo.GetAllDestinations();

            return Ok(destinations);
        }


        [HttpPost("addFlight/{companyId}")]
        public async Task<IActionResult> MakeNewFlight(int companyId, NewFlight newFlight)
        {
            var departureDest = _repo.GetDestination(newFlight.DepartureDestination);
            var arrivalDest = _repo.GetDestination(newFlight.ArrivalDestination);

            Flight flight = new Flight()
            {
                DepartureDestination = departureDest,
                ArrivalDestination = arrivalDest,
                AverageGrade = 6.6,
                DepartureTime = newFlight.DepartureTime,
                ArrivalTime = newFlight.ArrivalTime,
                Discount = false,
                Mileage = newFlight.TravelLength,
                TravelTime = newFlight.TravelDuration,
                TicketPrice = newFlight.Price
            };

            _repo.Add(flight);

            var companyFromRepo = _repo.GetCompanyWithFlights(companyId);
            companyFromRepo.Flights.Add(flight);

            if (await _repo.SaveAll())
                return NoContent();
            else
                throw new Exception("Saving flight failed on save!");
        }

        [HttpGet("getFlights/{companyId}")]
        public async Task<IActionResult> GetFlightsForCompany(int companyId, [FromQuery]FlightsParams flightsParams)
        {
            var flights = await _repo.GetFlightsForCompany(companyId, flightsParams);

            Response.AddPagination(flights.CurrentPage, flights.PageSize,
             flights.TotalCount, flights.TotalPages);

            return Ok(flights);
        }

        [HttpPost("addCompany")]
        [Authorize]
        public async Task<IActionResult> MakeNewCompany(CompanyToMake companyToMake)
        {
            Destination destination = new Destination();
            destination.City = companyToMake.City;
            destination.Country = companyToMake.Country;

            if (companyToMake.MapString.Length > 0)
                destination.MapString = destination.MapString;
            else
                destination.MapString = $"https://maps.google.com/maps?q={destination.City}&output=embed";

            destination.MapString = companyToMake.MapString;

            _repo.Add(destination);
            await _repo.SaveAll();

            AirCompany company = new AirCompany()
            {
                Name = companyToMake.Name,
                PromoDescription = "Temporary promo description",
                AverageGrade = 0,
                HeadOffice = destination,
            };

            _repo.Add(company);


            if (await _repo.SaveAll())
                return CreatedAtRoute("GetAvioCompany", new { id = company.Id }, company);
            else
                throw new Exception("Saving vehicle failed on save!");
        }

        

        [HttpGet("getDiscountedFlights/{companyId}")]
        public List<Flight> GetDiscountedFlights(int companyId)
        {
            return _repo.GetDiscountTicket(companyId);    
        }

        public async Task LoadAdmins()
        {
            List<AirCompany> companies = _repo.GetAllCompanies();
            List<User> users = await GetUsers();

            var company1 = companies.Where(x => x.Id == 1).FirstOrDefault();
            var company2 = companies.Where(x => x.Id == 2).FirstOrDefault();
            var company3 = companies.Where(x => x.Id == 3).FirstOrDefault();
            var company4 = companies.Where(x => x.Id == 4).FirstOrDefault();
            var company5 = companies.Where(x => x.Id == 5).FirstOrDefault();

            var user1 = users.Where(x => x.AuthId == "auth0|5ea9af38ee467e0c092c22d6").FirstOrDefault();
            var user2 = users.Where(x => x.AuthId == "auth0|5ea9af48ee467e0c092c22e6").FirstOrDefault();
            var user3 = users.Where(x => x.AuthId == "auth0|5ea9af5bee467e0c092c22fe").FirstOrDefault();
            var user4 = users.Where(x => x.AuthId == "auth0|5ea9af6aee467e0c092c231b").FirstOrDefault();
            var user5 = users.Where(x => x.AuthId == "auth0|5ea9af78ee467e0c092c2329").FirstOrDefault();

            _userRepo.Add(user1);
            _userRepo.Add(user2);
            _userRepo.Add(user3);
            _userRepo.Add(user4);
            _userRepo.Add(user5);

            await _userRepo.SaveAll();

            company1.Admin = user1;
            company2.Admin = user2;
            company3.Admin = user3;
            company4.Admin = user4;
            company5.Admin = user5;

            await _repo.SaveAll();

            return;
        }

        public async Task<List<User>> GetUsers()
        {
            var token = GetAuthorizationToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://pusgs.eu.auth0.com/api/v2/users");

            request.Headers.Add("Authorization", "Bearer " + token);

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            var toReturn = await response.Content.ReadAsStringAsync();

            List<UserFromServer> usersFromServer = JsonConvert.DeserializeObject<List<UserFromServer>>(toReturn);
            List<User> users = new List<User>();

            foreach (var us in usersFromServer)
            {
                User user = new User();
                user.AuthId = us.user_id;
                user.Email = us.email;

                if (us.user_metadata != null)
                {
                    user.FirstName = us.user_metadata.first_name;
                    user.LastName = us.user_metadata.last_name;
                }
                else
                {
                    user.FirstName = us.given_name;
                    user.LastName = us.family_name;
                }
                users.Add(user);
            }
            return users;
        }

        public static string GetAuthorizationToken()
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