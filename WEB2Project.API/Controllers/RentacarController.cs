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

        public RentacarController(IRentACarRepository repo, IHttpClientFactory clientFactory, IUsersRepository userRepo)
        {
            _repo = repo;
            _clientFactory = clientFactory;
            _userRepo = userRepo;
        }

        [HttpGet("{id}", Name = "GetRentACarCompany")]
        public async Task<IActionResult> GetRentACarCompany(int id)
        {
            var company = await _repo.GetCompany(id);
            await CheckCurrentDestination(id);

            return Ok(company);
        }

        [HttpGet("getVehicle/{id}", Name = "GetVehicle")]
        public IActionResult GetVehicle(int id)
        {
            var vehicle = _repo.GetVehicle(id);

            return Ok(vehicle);
        }

        [HttpPost("addNewBranch/{companyId}")]
        [Authorize]
        public async Task<IActionResult> AddNewDestination(int companyId, BranchToAdd branch)
        {
            var companyFromRepo = await _repo.GetCompany(companyId);

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
                throw new Exception("Adding branch failed on save!");
        }

        [HttpPost("editCompany")]
        [Authorize]
        public async Task<IActionResult> EditCompany(RentACarCompany company)
        {
            var companyFromRepo = await _repo.GetCompany(company.Id);

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
            branch.City = companyToMake.City;
            branch.Country = companyToMake.Country;

            if (companyToMake.MapString.Length > 0)
                branch.MapString = branch.MapString;
            else
            {
                var address = branch.Address.Replace(' ', '+');
                branch.MapString = $"https://maps.google.com/maps?q={address}&output=embed";
            }

            branch.MapString = companyToMake.MapString;

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
                throw new Exception("Saving vehicle failed on save!");
        }

        [HttpGet("carcompanies")]
        public async Task<IActionResult> GetRentACarCompanies()
        {
            var companies = _repo.GetAllCompanies();

            var company = companies.Where(x => x.Id == 1).FirstOrDefault();

            if (company.Admin == null)
            {
                await LoadAdmins();
            }

            return Ok(companies);
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
        public async Task<IActionResult> GetVehiclesForCompany(int companyId, [FromQuery]VehicleParams vehicleParams)
        {
            var vehicles = await _repo.GetVehiclesForCompany(companyId, vehicleParams);

            Response.AddPagination(vehicles.CurrentPage, vehicles.PageSize,
             vehicles.TotalCount, vehicles.TotalPages);

            return Ok(vehicles);
        }

        [HttpGet("getDiscountedVehicles/{companyId}")]
        [Authorize]
        public IActionResult GetDiscountedVehicles(int companyId)
        {
            var discountedVehicles = _repo.GetDiscountedVehicles(companyId);

            return Ok(discountedVehicles);
        }

        [HttpPost("getIncomes/{companyid}", Name = "GetCompanyIncomes")]
        [Authorize]
        public async Task<IActionResult> GetCompanyIncomes(int companyid, IncomeData data)
        {
            var company = await _repo.GetCompany(companyid);

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
        public async Task<IActionResult> MakeNewVehicle (int companyId, Vehicle vehicleFromBody)
        {
            var company = await _repo.GetCompany(companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            Vehicle vehicle = new Vehicle()
            {
                Manufacturer = vehicleFromBody.Manufacturer,
                Model = vehicleFromBody.Model,
                AverageGrade = 0,
                Ratings = new List<VehicleRating>(),
                Doors = vehicleFromBody.Doors,
                Seats = vehicleFromBody.Seats,
                CurrentDestination = vehicleFromBody.CurrentDestination,
                Price = vehicleFromBody.Price,
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
                throw new Exception("Saving vehicle failed on save!");
        }

        [HttpPost("editVehicle/{vehicleId}/{companyId}")]
        [Authorize]
        public async Task<IActionResult> EditVehicle(int vehicleId, int companyId, Vehicle vehicleFromBody)
        { 
            var company = await _repo.GetCompany(companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

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

        [HttpPost("deleteVehicle/{vehicleId}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int vehicleId, [FromBody]JObject data)
        {
            var vehicle = _repo.GetVehicle(vehicleId);
            int companyId = Int32.Parse(data["companyId"].ToString());

            var company = await _repo.GetCompany(companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();


            vehicle.IsDeleted = true;

            if (await _repo.SaveAll())
                return Ok();
            else
                throw new Exception("Deleting vehicle failed on save!");
        }

        [HttpPost("rateVehicle/{vehicleId}")]
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<IActionResult> ChangeHeadOffice (int companyId, [FromBody]JObject data)
        {
            var company = await _repo.GetCompany(companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
             User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var headOffice = data["headOffice"].ToString();

            if (company.HeadOffice.City == headOffice)
                return NoContent();

            var destination = company.Branches.Where(d => d.City == headOffice).FirstOrDefault();
            company.HeadOffice = destination;

            await _repo.SaveAll(); 

            return Ok();
        }

        [HttpPost("changeVehicleLocation/{vehicleId}")]
        [Authorize]
        public async Task<IActionResult> ChangeVehicleLocation(int vehicleId, [FromBody]JObject data)
        {
            var vehicle = _repo.GetVehicle(vehicleId);
            var companyId = Int32.Parse(data["companyId"].ToString());
            var company = await _repo.GetCompany(companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
               User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
               User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var newCity = data["newCity"].ToString();

            if (vehicle.CurrentDestination.ToLower() == newCity.ToLower())
                return NoContent();

            vehicle.CurrentDestination = newCity;

            await _repo.SaveAll();

            return Ok();
        }

        [HttpPost("removeDestination/{companyId}")]
        [Authorize]
        public async Task<IActionResult> RemoveDestination(int companyId, [FromBody]JObject data)
        {
            var company = await _repo.GetCompany(companyId);

            if (User.FindFirst(ClaimTypes.NameIdentifier).Value != company.Admin.AuthId &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin1 &&
                User.FindFirst(ClaimTypes.NameIdentifier).Value != SystemAdminData.SysAdmin2)
                return Unauthorized();

            var location = data["location"].ToString();

            if (company.HeadOffice.City == location)
                return NoContent();

            var branch = company.Branches.Where(d => d.City == location).FirstOrDefault();

            company.Branches.Remove(branch);

            await _repo.SaveAll();

            return Ok();
        }

        [HttpGet("canEdit/{vehicleId}")]
        [Authorize]
        public IActionResult CanBeEditedOrDeleted(int vehicleId)
        {
            var vehicle = _repo.GetVehicle(vehicleId);

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
        public async Task<IActionResult> CanRemoveLocation(int companyId, [FromBody]JObject data)
        {
            var company = await _repo.GetCompanyWithVehicles(companyId);
            var location = data["location"].ToString();

            bool flag = true;

            foreach (var v in company.Vehicles)
            {
                if (v.CurrentDestination.ToLower() == location.ToLower())
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
            var reservations = _repo.GetCompanyReservations(companyId);

            foreach(var r in reservations)
            {
                if(r.EndDate.Date <= DateTime.Now.Date)
                {
                    r.Vehicle.CurrentDestination = r.ReturningLocation;
                }
            }
            await _repo.SaveAll();
        }

    }
}