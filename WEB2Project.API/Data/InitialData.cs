using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
using WEB2Project.API.Models;
using WEB2Project.Models;

namespace WEB2Project.Data
{
    public class InitialData
    {
        public static void SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            if (!userManager.Users.Any())
            {
                var userData = System.IO.File.ReadAllText("Data/UserSeedData.json");
                var users = JsonConvert.DeserializeObject<List<User>>(userData);

                //create some roles

                var roles = new List<Role>
                {
                    new Role{Name = "Member"},
                    new Role{Name = "Admin"},
                    new Role{Name = "Moderator"},
                };

                foreach (var role in roles)
                {
                    roleManager.CreateAsync(role).Wait();
                }

                foreach (var user in users)
                {
                    userManager.CreateAsync(user, "password").Wait();
                    userManager.AddToRoleAsync(user, "Member");
                }

                //create admin user

                var adminUser = new User
                {
                    UserName = "Admin"
                };

                var result = userManager.CreateAsync(adminUser, "password").Result;

                if (result.Succeeded)
                {
                    var admin = userManager.FindByNameAsync("Admin").Result;
                    userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
                }
            }
        }
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.EnsureCreated();

                if (context.AirCompanies.Any())
                    return;

                var aircompanies = GetAirCompanies().ToArray();
                context.AirCompanies.AddRange(aircompanies);
                context.SaveChanges();

                var rentacarcompanies = GetRentACarCompanies().ToArray();
                context.RentACarCompanies.AddRange(rentacarcompanies);
                context.SaveChanges();

                var destinations = GetDestinations().ToArray();
                context.Destinations.AddRange(destinations);
                context.SaveChanges();

                var flights = GetFlights().ToArray();
                context.Flights.AddRange(flights);
                context.SaveChanges();
            }
        }

        public static List<AirCompany> GetAirCompanies()
        {
            List<AirCompany> airCompanies = new List<AirCompany>()
            {
                new AirCompany {Name = "Qatar Airways", Address="Doha, Qatar", AverageGrade = 10,
                    PromoDescription = "We are in this together"},
                new AirCompany {Name = "Singapore Airlines", Address="Singapore, Singapore", AverageGrade = 9.2,
                    PromoDescription = "Enjoy world-class service"},
                new AirCompany {Name = "Emirates", Address="Dubai, UAE", AverageGrade = 8.9,
                    PromoDescription = "Choose Emirates airline to enjoy our world-class service on all flights"},
                new AirCompany {Name = "Lufthansa", Address="Koln, Germany", AverageGrade = 8.4,
                    PromoDescription = "The Lufthansa Group is an aviation group with operations worldwide"},
                new AirCompany {Name = "Air Serbia", Address="Belgrade, Serbia", AverageGrade = 7.6,
                    PromoDescription = "Air Serbia has been a leader in air transport since the company was founded in 1927"}
            };

            return airCompanies;
        }

        public static List<RentACarCompany> GetRentACarCompanies()
        {
            List<RentACarCompany> rentACarCompanies = new List<RentACarCompany>()
            {
                new RentACarCompany {Name = "Alamo", Address="Munchen, Germany", AverageGrade = 8.6,
                    PromoDescription = "Dropping a car off with Alamo is quick and easy"},
                new RentACarCompany {Name = "Europcar", Address="Milano, Italy", AverageGrade = 9.2,
                    PromoDescription = "Enjoy world-class service"},
                new RentACarCompany {Name = "Hertz", Address="Athens, Greece", AverageGrade = 8.9,
                    PromoDescription = "Highly recommended by our customers"},
                new RentACarCompany {Name = "Avis", Address="Paris, France", AverageGrade = 8.4,
                    PromoDescription = "Rated by more than 3.5 million people"},
                new RentACarCompany {Name = "Dollar", Address="New York, USA", AverageGrade = 7.6,
                    PromoDescription = "World's biggest online car rental service"}
            };

            return rentACarCompanies;
        }

        public static List<Destination> GetDestinations()
        {
            List<Destination> destinations = new List<Destination>()
            { 
                new Destination {City = "Belgrade", Country = "Serbia"},
                new Destination {City = "Milan", Country = "Italy"},
                new Destination {City = "Vienna", Country = "Austria"},
                new Destination {City = "Malmo", Country = "Sweden"},
                new Destination {City = "Berlin", Country = "Germany"},
                new Destination {City = "Las Vegas", Country = "USA"},
                new Destination {City = "Frankfurt", Country = "Germany"},
                new Destination {City = "Paris", Country = "France"},
                new Destination {City = "Moscow", Country = "Russia"},
                new Destination {City = "Oslo", Country = "Norway"},
                new Destination {City = "Budapest", Country = "Hungary"},
                new Destination {City = "Novi Sad", Country = "Serbia"},
            };

            return destinations;
        }

        public static List<Flight> GetFlights()
        {
            Random random = new Random();
            List<string> cities = new List<string>();
            cities.Add("Belgrade");
            cities.Add("Milan");
            cities.Add("Vienna");
            cities.Add("Malmo");
            cities.Add("Berlin");
            cities.Add("Las Vegas");
            cities.Add("Frankfurt");
            cities.Add("Paris");
            cities.Add("Moscow");
            cities.Add("Oslo");
            cities.Add("Budapest");
            cities.Add("Novi Sad");

            List<Flight> flights = new List<Flight>();

            for (int i = 0; i < 50; i++)
            {
                var companyId = random.Next(1, 5);
                var departureCity = cities[random.Next(1, 12)];
                var arrivalCity = cities[random.Next(1, 12)];
                var departureTime = DateTime.Now.AddDays(random.Next(1, 15)).AddHours(random.Next(1, 14)).AddMinutes(random.Next(1, 59)); 
                var arrivalTime = departureTime.AddHours(random.Next(1, 3)).AddMinutes(random.Next(1, 59));


                Flight flight = new Flight {AirCompanyId = companyId, DepartureCity = departureCity, 
                    ArrivalCity = arrivalCity, DepartureTime = departureTime, ArrivalTime = arrivalTime };

                if(departureCity != arrivalCity)
                {
                    flights.Add(flight);
                }
            }

            return flights;
        }
    }


}

