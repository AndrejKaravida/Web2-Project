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
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RentacarController : ControllerBase
    {
        private readonly IRentACarRepository _repo;
        private readonly IUsersRepository _userRepo;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IMapper _mapper;

        public RentacarController(IRentACarRepository repo, IHttpClientFactory clientFactory,
            IUsersRepository userRepo, IMapper mapper)
        {
            _repo = repo;
            _clientFactory = clientFactory;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        [HttpPost("{criteria}")]
        public async Task<IActionResult> GetCompaniesWithCriteria([FromBody] SearchParams searchParams)
        {
            var companies = await _repo.GetCompaniesWithCriteria(searchParams);

            var companiesToReturn = _mapper.Map<List<CompanyToReturn>>(companies);
            return Ok(companiesToReturn);
        }

        [HttpGet("branches")]
        public async Task<IActionResult> GetAllBranches()
        {
            var branches = await _repo.GetBranches();

            return Ok(branches);
        }

        [HttpGet("{id}", Name = "GetRentACarCompany")]
        public async Task<IActionResult> GetRentACarCompany(int id)
        {
            await CheckCurrentDestination(id);
            var company = await _repo.GetCompany(id);

            var companyToReturn = _mapper.Map<CompanyToReturn>(company);
            return Ok(companyToReturn);
        }

        [HttpPost("addNewBranch/{companyId}")]
        [Authorize]
        public async Task<IActionResult> AddNewDestination(int companyId, BranchToAdd branch)
        {
            var companyFromRepo = await _repo.GetCompany(companyId);

            if(companyFromRepo == null)
            {
                return NoContent();
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != companyFromRepo.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            Branch newBranch = new Branch()
            {
                City = branch.City,
                Country = branch.Country,
                Address = branch.Address
            };

            if (branch.MapString.Length > 0)
                newBranch.MapString = branch.MapString;
            else
            {
                var address = branch.Address.Replace(' ', '+');
                newBranch.MapString = $"https://maps.google.com/maps?q={address}&output=embed";
            }

            _repo.Add(newBranch);
            await _repo.SaveAll();

            companyFromRepo.Branches.Add(newBranch);

            if (await _repo.SaveAll())
                return Ok();
            else
                return BadRequest("Adding destination failed on save");
        }

        [HttpPost("editCompany")]
        [Authorize]
        public async Task<IActionResult> EditCompany(CompanyToEdit company)
        {
            var companyFromRepo = await _repo.GetCompany(company.Id);

            if (companyFromRepo == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            companyFromRepo.Name = company.Name;
            companyFromRepo.PromoDescription = company.PromoDescription;
            companyFromRepo.MonthRentalDiscount = company.MonthRentalDiscount;
            companyFromRepo.WeekRentalDiscount = company.WeekRentalDiscount;

            await _repo.SaveAll();

            return Ok();
        }

        [HttpPost("addCompany")]
        [Authorize]
        public async Task<IActionResult> MakeNewCompany(CompanyToMake companyToMake)
        {
            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

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

            RentACarCompany company = new RentACarCompany()
            {
                Name = companyToMake.Name,
                PromoDescription = "Temporary promo description",
                AverageGrade = 0,
                Branches = new List<Branch>(),
                HeadOffice = branch,
                WeekRentalDiscount = 0,
                MonthRentalDiscount = 0
            };

            company.Branches.Add(branch);

            _repo.Add(company);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetRentACarCompany", new { id = company.Id }, company);
            else
                return BadRequest("Making new company failed on save");
        }

        [HttpGet("carcompanies")]
        public async Task<IActionResult> GetRentACarCompanies()
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

            var companiesToReturn = _mapper.Map<List<CompanyToReturn>>(companies);
            return Ok(companiesToReturn);
        }

        public async Task LoadAdmins()
        {
            List<RentACarCompany> companies = _repo.GetAllCompanies();
            List<User> users = await GetUsers();

            var company1 = companies.Where(x => x.Id == 1).FirstOrDefault();
            var company2 = companies.Where(x => x.Id == 2).FirstOrDefault();
            var company3 = companies.Where(x => x.Id == 3).FirstOrDefault();
            var company4 = companies.Where(x => x.Id == 4).FirstOrDefault();
            var company5 = companies.Where(x => x.Id == 5).FirstOrDefault();

            var user1 = users.Where(x => x.AuthId == "auth0|5ea9ae62834d0c0c1f7855d0").FirstOrDefault();
            var user2 = users.Where(x => x.AuthId == "auth0|5ea9aef6834d0c0c1f785672").FirstOrDefault();
            var user3 = users.Where(x => x.AuthId == "auth0|5ea9af05834d0c0c1f785684").FirstOrDefault();
            var user4 = users.Where(x => x.AuthId == "auth0|5ea9af14834d0c0c1f78569b").FirstOrDefault(); 
            var user5 = users.Where(x => x.AuthId == "auth0|5ea9af27834d0c0c1f7856b3").FirstOrDefault();

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

        [HttpGet("getVehicles/{companyId}")]
        public IActionResult GetVehiclesForCompany(int companyId, [FromQuery]VehicleParams vehicleParams)
        {
            var vehicles = _repo.GetVehiclesForCompany(companyId, vehicleParams);

            if (vehicles == null)
            {
                return NoContent();
            }

            Response.AddPagination(vehicles.CurrentPage, vehicles.PageSize,
             vehicles.TotalCount, vehicles.TotalPages);

            var vehiclesToReturn = _mapper.Map <List<VehicleToReturn>>(vehicles);

            return Ok(vehiclesToReturn);
        }

        [HttpGet("getDiscountedVehicles/{companyId}")]
        [Authorize]
        public IActionResult GetDiscountedVehicles(int companyId)
        {
            var discountedVehicles = _repo.GetDiscountedVehicles(companyId);

            if (discountedVehicles == null)
            {
                return NoContent();
            }

            var vehiclesToReturn = _mapper.Map<List<VehicleToReturn>>(discountedVehicles);

            return Ok(vehiclesToReturn);
        }

        [HttpPost("getIncomes/{companyid}", Name = "GetCompanyIncomes")]
        [Authorize]
        public async Task<IActionResult> GetCompanyIncomes(int companyid, IncomeData data)
        {
            var company = await _repo.GetCompany(companyid);

            if (company == null)
            {
                return NoContent();
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var incomes = _repo.GetCompanyIncomes(companyid);

            incomes = incomes.Where(x => x.Date.Date >= data.StartingDate && x.Date.Date <= data.FinalDate).ToList();

            List<DateTime> dates = new List<DateTime>();

            for(var dt = data.StartingDate; dt <= data.FinalDate; dt = dt.AddDays(1))
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
                incomeDates.Add(dates[kvp.Key].ToString("dd/MM/yyyy", CultureInfo.InvariantCulture));
            }

            IncomeStatsToReturn incomeStatsToReturn = new IncomeStatsToReturn();
            incomeStatsToReturn.values = incomeValues.ToArray();
            incomeStatsToReturn.dates = incomeDates.ToArray();

            return Ok(incomeStatsToReturn);
        }

        [HttpGet("getReservations/{companyid}", Name = "GetCompanyReservations")]
        [Authorize]
        public async Task<IActionResult> GetCompanyReservartions(int companyid)
        {
            var company = await _repo.GetCompany(companyid);

            if (company == null)
            {
                return NoContent();
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

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
        [Authorize]
        public async Task<IActionResult> MakeNewVehicle (int companyId, VehicleToMake vehicleFromBody)
        {
            var company = await _repo.GetCompany(companyId);

            if (company == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            Vehicle vehicle = new Vehicle()
            {
                Manufacturer = vehicleFromBody.Manufacturer,
                Model = vehicleFromBody.Model,
                AverageGrade = 7,
                Ratings = new List<VehicleRating>(),
                Doors = vehicleFromBody.Doors,
                Seats = vehicleFromBody.Seats,
                CurrentDestination = vehicleFromBody.CurrentDestination,
                Price = Int32.Parse(vehicleFromBody.Price),
                IsDeleted = false,
                Photo = "",
                Type = vehicleFromBody.Type
            };

            var companyFromRepo = await _repo.GetCompanyWithVehicles(companyId);
            vehicle.CurrentDestination = companyFromRepo.HeadOffice.City;

            _repo.Add(vehicle);

            companyFromRepo.Vehicles.Add(vehicle);

            if (await _repo.SaveAll())
                return CreatedAtRoute("GetVehicle", new { id = vehicle.Id }, vehicle);
            else
                return BadRequest("Making new vehicle failed on save");
        }

        [HttpPost("editVehicle/{vehicleId}/{companyId}")]
        [Authorize]
        public async Task<IActionResult> EditVehicle(int vehicleId, int companyId, VehicleToMake vehicleFromBody)
        { 
            var company = await _repo.GetCompany(companyId);

            if (company == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var vehicle = _repo.GetVehicle(vehicleId);
            vehicle.Manufacturer = vehicleFromBody.Manufacturer;
            vehicle.Model = vehicleFromBody.Model;
            vehicle.Doors = vehicleFromBody.Doors;
            vehicle.Seats = vehicleFromBody.Seats;
            vehicle.Price = Int32.Parse(vehicleFromBody.Price);
            vehicle.Type = vehicleFromBody.Type;

            await _repo.SaveAll();
            return NoContent();
   
        }

        [HttpPost("deleteVehicle/{vehicleId}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int vehicleId, [FromBody]DeleteVehicle data)
        {
            var company = await _repo.GetCompany(data.CompanyId);

            if (company == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var vehicle = _repo.GetVehicle(vehicleId);

            if(vehicle == null)
            {
                return BadRequest("Cannot find vehicle with id provided!");
            }

            vehicle.IsDeleted = true;

            if (await _repo.SaveAll())
                return Ok();
            else
                return BadRequest("Deleting vehicle failed on save");
        }

        [HttpPost("rate")]
        [Authorize]
        public async Task<IActionResult> Rate([FromBody]RateData data)
        {
            var company = await _repo.GetCompany(data.CompanyId);

            if(company == null)
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

            var vehicle = _repo.GetVehicle(data.VehicleId);

            if (vehicle == null)
            {
                return BadRequest("Cannot find vehicle with id provided!");
            }

            VehicleRating newVehicleRating = new VehicleRating() { Value = data.VehicleRating, UserId = data.UserId };
            vehicle.Ratings.Add(newVehicleRating);

            double vehicleratingsCount = vehicle.Ratings.Count;

            double vehicletotalRatings = 0;

            foreach (var r in vehicle.Ratings)
            {
                vehicletotalRatings += r.Value;
            }

            double vehicleaverageRating = vehicletotalRatings / vehicleratingsCount;

            vehicle.AverageGrade = Math.Round(vehicleaverageRating, 2);

            if (await _repo.SaveAll())
                return Ok();
            else
                return BadRequest("Rating failed on save");

        }

        [HttpPost("changeHeadOffice/{companyId}")]
        [Authorize]
        public async Task<IActionResult> ChangeHeadOffice (int companyId, [FromBody]ChangeHeadOffice data)
        {
            var company = await _repo.GetCompany(companyId);

            if (company == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            if (company.HeadOffice.City == data.HeadOffice)
                return NoContent();

            var destination = company.Branches.Where(d => d.City == data.HeadOffice).FirstOrDefault();
            company.HeadOffice = destination;

            await _repo.SaveAll(); 

            return Ok();
        }

        [HttpPost("changeVehicleLocation/{vehicleId}")]
        [Authorize]
        public async Task<IActionResult> ChangeVehicleLocation(int vehicleId, [FromBody]ChangeVehicleLocation data)
        {
            var company = await _repo.GetCompany(data.CompanyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
               User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
               User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var vehicle = _repo.GetVehicle(vehicleId);

            if (vehicle == null)
            {
                return BadRequest("Cannot find vehicle with id provided!");
            }

            if (vehicle.CurrentDestination.ToLower() == data.NewCity.ToLower())
                return NoContent();

            vehicle.CurrentDestination = data.NewCity;

            await _repo.SaveAll();

            return Ok();
        }

        [HttpPost("removeDestination/{companyId}")]
        [Authorize]
        public async Task<IActionResult> RemoveDestination(int companyId, [FromBody]RemoveDestination data)
        {
            var company = await _repo.GetCompany(companyId);

            if(company == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            if (company.HeadOffice.City == data.Location)
                return NoContent();

            var branch = company.Branches.Where(d => d.City == data.Location).FirstOrDefault();

            company.Branches.Remove(branch);

            await _repo.SaveAll();

            return Ok();
        }

        [HttpGet("canEdit/{vehicleId}")]
        [Authorize]
        public IActionResult CanBeEditedOrDeleted(int vehicleId)
        {
            var vehicle = _repo.GetVehicle(vehicleId);

            if (vehicle == null)
            {
                return BadRequest("Cannot find vehicle with id provided!");
            }

            bool flag = true;

            foreach(var rd in vehicle.ReservedDates)
            {
                if(rd.Date.Day > DateTime.Now.Day)
                {
                    flag = false;
                    break;
                }
            }

            return Ok(flag);
        }

        [HttpPost("canRemoveLocation/{companyId}")]
        public async Task<IActionResult> CanRemoveLocation(int companyId, [FromBody]RemoveDestination data)
        {
            var company = await _repo.GetCompanyWithVehicles(companyId);

            if (company == null)
            {
                return BadRequest("Cannot find company with id provided!");
            }

            bool flag = true;

            foreach (var v in company.Vehicles)
            {
                if (v.CurrentDestination.ToLower() == data.Location.ToLower())
                {
                    flag = false;
                    break;
                }
            }

            return Ok(flag);
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

        public async Task CheckCurrentDestination(int companyId)
        {
            var reservations = _repo.GetCompanyReservations(companyId).Where(x => x.EndDate.Date <= DateTime.Now.Date).ToList();

            foreach(var r in reservations)
            {
              r.Vehicle.CurrentDestination = r.ReturningLocation;
            }

            await _repo.SaveAll();
        }

    }
}