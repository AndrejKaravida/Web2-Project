using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Dtos;
using WEB2Project.Helpers;
using WEB2Project.Models;
using WEB2Project.Models.AircompanyModels;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvioController : ControllerBase
    {
        private readonly IFlightsRepository _repo;
        private readonly IUsersRepository _userRepo;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public AvioController(IFlightsRepository repo, IUsersRepository userRepo, 
            IHttpClientFactory clientFactory, IMapper mapper)
        {
            _repo = repo;
            _userRepo = userRepo;
            _clientFactory = clientFactory;
            _mapper = mapper;
        }

        [HttpGet("getCompany/{id}", Name = "GetAvioCompany")]
        public IActionResult GetAvioCompany(int id)
        {
            var company = _repo.GetCompany(id);

           var companyToReturn = _mapper.Map<AirCompanyToReturn>(company);
            return Ok(companyToReturn);
        }

        [HttpPost("editHeadOffice/{companyId}")]
        //[Authorize]
        public async Task<IActionResult> EditHeadOffice(int companyId, [FromBody]JObject data)
        {
            var company = _repo.GetCompany(companyId);
            var headOffice = data["headOffice"].ToString();

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
           User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
           User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            if (company.HeadOffice.City == headOffice)
                return NoContent();

            var branches = _repo.GetAllBranches();
            var branch = branches.Where(x => x.City == headOffice).FirstOrDefault();
   
            company.HeadOffice = branch;

            await _repo.SaveAll();

            return Ok();
        }

        [HttpGet("aircompanies")]
        public async Task <IActionResult> GetAllCompanies()
        {
            var companies = _repo.GetAllCompanies();

            if (companies == null)
            {
                return NoContent();
            }

            var company = companies.Where(x => x.Id == 1).FirstOrDefault();

            if (company.Admin == null)
            {
                await LoadAdmins();
            }

            var companiesToReturn = _mapper.Map<List<AirCompanyToReturn>>(companies);
            return Ok(companiesToReturn);
        }

        [HttpPost("editFlight/{flightId}")]
        public async Task<IActionResult> EditFlight(int flightId,FlightToEdit flightToEdit)
        {
            var flight = _repo.GetFlight(flightId);
            var company = _repo.GetCompany(flightToEdit.companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
            User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
            User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();


            //flight.DepartureTime = flightToEdit.DepartureTime;
            //flight.ArrivalTime = flightToEdit.ArrivalTime;
            flight.TravelTime = flightToEdit.TravelTime;
            flight.Mileage = flightToEdit.Mileage;
            flight.Luggage = flightToEdit.Luggage;
            flight.TicketPrice = flightToEdit.TicketPrice;


            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Editing flight failed on save!");


        }

        [HttpGet("checkcompany/{id}")]
        public IActionResult CheckCompany (int id)
        {
            var compAirCompanyToReturn = _mapper.Map<AirCompanyToReturn>(_repo.GetCompanyForFlight(id));
            return Ok(compAirCompanyToReturn);
        }

        [HttpPost("editcompany/{copmanyId}")]
        public async Task<IActionResult>EditCompany(int copmanyId, AirCompany companyToEdit)
        {
            var company =  _repo.GetCompany(copmanyId);
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            company.Name = companyToEdit.Name;
            company.PromoDescription = companyToEdit.PromoDescription;

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Editing company failed on save!");
        }

        [HttpPost("avioIncomes/{companyId}")]
        [Authorize]
        public async Task<IActionResult> GetAvioIncomes(int companyId, IncomeData data)
        {
            var company = _repo.GetCompany(companyId);
            if(company == null)
            {
                return null;
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
               User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
               User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
              return Unauthorized();

            var incomes = _repo.GetAvioIncomes(companyId);
            incomes = incomes.Where(x => x.Date.Date >= data.StartingDate && x.Date.Date <= data.FinalDate).ToList();

            List<DateTime> dates = new List<DateTime>();

            for (var dt = data.StartingDate; dt <= data.FinalDate; dt = dt.AddDays(1))
            {
                dates.Add(dt);
            }

            Dictionary<int, double> keyValuePairs = new Dictionary<int, double>();

            for (int i = 0; i < dates.Count; i++)
            {
                keyValuePairs.Add(i, 0);
            }

            for (int i = 0; i < dates.Count; i++)
            {
                foreach (var income in incomes)
                {
                    if (income.Date.Date == dates[i])
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
                incomeDates.Add(dates[kvp.Key].ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            }

            IncomeStatsToReturn incomeStatsToReturn = new IncomeStatsToReturn();
            incomeStatsToReturn.values = incomeValues.ToArray();
            incomeStatsToReturn.dates = incomeDates.ToArray();

            return Ok(incomeStatsToReturn);

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
                Luggage = newFlight.Luggage,
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
        public IActionResult GetFlightsForCompany(int companyId, [FromQuery]FlightsParams flightsParams)
        {
            var flights = _repo.GetFlightsForCompany(companyId, flightsParams);

            if(flights == null)
            {
                return NoContent();
            }

            Response.AddPagination(flights.CurrentPage, flights.PageSize,
             flights.TotalCount, flights.TotalPages);

            return Ok(flights);
        }

        [HttpPost("addCompany")]
        [Authorize]
        public async Task<IActionResult> MakeNewCompany(CompanyToMake companyToMake)
        {
            Branch branch = new Branch();
            branch.Address = companyToMake.Address;
            branch.City = companyToMake.City;
            branch.Country = companyToMake.Country;

            if (companyToMake.MapString.Length > 0)
                branch.MapString = branch.MapString;
            else
            {
                var address = branch.Address.Replace(' ', '+');
                branch.MapString = $"https://maps.google.com/maps?q={address}&output=embed";
            }

            _repo.Add(branch);
            await _repo.SaveAll();

            AirCompany company = new AirCompany()
            {
                Name = companyToMake.Name,
                PromoDescription = "Temporary promo description",
                AverageGrade = 0,
                HeadOffice = branch
            };

            _repo.Add(company);


            if (await _repo.SaveAll())
                return CreatedAtRoute("GetAvioCompany", new { id = company.Id }, company);
            else
                return BadRequest();
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

        [HttpPost("searchFlights")]
        public async Task<IActionResult> SearchFlights([FromBody] FlightDto dataFromClient)
        {
            var flights = await _repo.GetFlights(dataFromClient);

            return Ok(flights);
        }

        [HttpPost("rate")]
        [Authorize]
        public async Task<IActionResult> Rate([FromBody]RateFlightData data)
        {
            var company = _repo.GetCompany(data.CompanyId);

            if (company == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            var userId = User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var reservation = _repo.GetReservation(data.ReservationId);

            if (reservation.UserAuthId != userId)
            {
                return BadRequest("You can rate only your reservations!");
            }

            if (reservation.Status == "Finished")
            {
                return BadRequest("You cannot leave double rate for the same reservation!");
            }

            reservation.Status = "Finished";

            CompanyRating newRating = new CompanyRating() { Value = data.CompanyRating, UserId = data.UserId };
            company.Ratings.Add(newRating);

            double ratingsCount = company.Ratings.Count;

            double totalRatings = 0;

            foreach (var r in company.Ratings)
            {
                totalRatings += r.Value;
            }

            double averageRating = totalRatings / ratingsCount;

            company.AverageGrade = Math.Round(averageRating, 2);

            var flight = _repo.GetFlight(data.FlightId);

            if (flight == null)
            {
                return BadRequest("Cannot find flight with id provided!");
            }

            FlightRating newFlightRating = new FlightRating() { Value = data.FlightRating, UserId = data.UserId };
            flight.Ratings.Add(newFlightRating);

            double flightratingsCount = flight.Ratings.Count;

            double flighttotalRatings = 0;

            foreach (var r in flight.Ratings)
            {
               flighttotalRatings += r.Value;
            }

            double flightaverageRating = flighttotalRatings / flightratingsCount;

            flight.AverageGrade = Math.Round(flightaverageRating, 2);

            if (await _repo.SaveAll())
                return Ok();
            else
                return BadRequest("Rating failed on save");

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