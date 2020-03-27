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
using WEB2Project.Models.RentacarModels;

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

                var locations = GetLocations().ToArray();
                context.Locations.AddRange(locations);
                context.SaveChanges();

                var vehicles = GetVehicles().ToArray();
                context.Vehicles.AddRange(vehicles);
                context.SaveChanges();

                var rentacarcompanies = GetRentACarCompanies(context).ToArray();
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

        public static List<RentACarCompany> GetRentACarCompanies(DataContext db)
        {
            var locations = GetLocations();

            List<RentACarCompany> rentACarCompanies = new List<RentACarCompany>()
            {
                new RentACarCompany {Name = "Ace rental", Address = "Thermae 156/18, Brussel, Brussel, Belgium", AverageGrade = 9.1,
                    WeekRentalDiscount = 10, MonthRentalDiscount = 40, Incomes = null, PromoDescription = "The best Rental in town!",
                    Locations = new List<Location>{locations[0], locations[3], locations[6]}, Vehicles = new List<Vehicle>(db.Vehicles.Take(6))},
                new RentACarCompany {Name = "Car rentals", Address = "Cascata delle Marmore Belvedere Superiore 4, Roma, Italy", AverageGrade = 9.4,
                    WeekRentalDiscount = 15, MonthRentalDiscount = 45, Incomes = null, PromoDescription = "Drive with professionals",
                    Locations = new List<Location>{locations[1], locations[4]}, Vehicles = new List<Vehicle>(db.Vehicles.Skip(6).Take(6))},
                new RentACarCompany {Name = "Experience rentals", Address = "Birlik Mosque 59, Ankara, Turkey", AverageGrade = 9.1,
                    WeekRentalDiscount = 12, MonthRentalDiscount = 39, Incomes = null, PromoDescription = "Experience is in our name",
                    Locations = new List<Location>{locations[2], locations[5], locations[7]}, Vehicles = new List<Vehicle>(db.Vehicles.Skip(12).Take(5))}
            };

            return rentACarCompanies;
        }

        public static List<Location> GetLocations()
        {
            List<Location> locations = new List<Location>()
            { 
                new Location {Address = "Thermae 156/18, Brussel", Country = "Belgium"},
                new Location {Address = "Cascata delle Marmore Belvedere Superiore 4, Roma", Country = "Italy"},
                new Location {Address = "Birlik Mosque 59, Ankara", Country = "Turkey"},
                new Location {Address = "Hühnermoor 18, Hamburg", Country = "Germany"},
                new Location {Address = "Elbterrassen zu Brambach 134, Leipzig", Country = "Germany"},
                new Location {Address = "Castillo de Pedraza 12, Madrid", Country = "Spain "},
                new Location {Address = "Aux Délices d'Angerville 14, Nantes", Country = "France"},
                new Location {Address = "Chateau de Bourbon-L'Archambault 36A, Paris", Country = "France"}
            };

            return locations;
        }

        public static List<Vehicle> GetVehicles()
        {
            List<Vehicle> vehicles = new List<Vehicle>()
            {
                new Vehicle {Manufacturer = "Alfa Romeo", Model = "Giulia", AverageGrade = 8.6, Year = 2016, Doors = 4, Seats = 5, Price = 369, Photo = "http://localhost:5000/1.jpg", Type = "Medium"},
                new Vehicle {Manufacturer = "Alfa Romeo", Model = "Quadrifoglio", AverageGrade = 8.8, Year = 2020 , Doors = 4, Seats = 5, Price = 158, Photo = "http://localhost:5000/2.jpg" , Type = "Medium"},
                new Vehicle {Manufacturer = "Audi", Model = "A5 Sportback", AverageGrade = 9.6, Year = 2018, Doors = 2, Seats = 2, Price = 347, Photo = "http://localhost:5000/3.jpg", Type = "Luxury, Medium"},
                new Vehicle {Manufacturer = "Audi", Model = "A6", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, Price = 395, Photo = "http://localhost:5000/4.jpg", Type = "Large, Luxury"},
                new Vehicle {Manufacturer = "Audi", Model = "A7", AverageGrade = 8.6,  Year = 2016, Doors = 4, Seats = 5, Price = 390, Photo = "http://localhost:5000/5.jpg", Type = "Large, Luxury"},
                new Vehicle {Manufacturer = "Audi", Model = "A8", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, Price = 399, Photo = "http://localhost:5000/6.jpg", Type = "Large, Luxury"},
                new Vehicle {Manufacturer = "Genesis", Model = "G70", AverageGrade = 7.9,  Year = 2018, Doors = 4, Seats = 5, Price = 260, Photo = "http://localhost:5000/7.jpg", Type = "Medium"},
                new Vehicle {Manufacturer = "BMW ", Model = "2-series", AverageGrade = 6.6, Year = 2015, Doors = 2, Seats = 5, Price = 290, Photo = "http://localhost:5000/8.jpg", Type = "Medium"},
                new Vehicle {Manufacturer = "Chevrolet", Model = "Corvette", AverageGrade = 7.9,  Year = 2016, Doors = 2, Seats = 5, Price = 390, Photo = "http://localhost:5000/10.jpg", Type = "Small, Luxury"},
                new Vehicle {Manufacturer = "Ford", Model = "Mustang", AverageGrade = 8.6, Year = 2014, Doors = 4, Seats = 5, Price = 380, Photo = "http://localhost:5000/11.jpg", Type = "Medium, Luxury"},
                new Vehicle {Manufacturer = "Honda", Model = "Accord", AverageGrade = 7.6, Year = 2016, Doors = 4, Seats = 5, Price = 130, Photo = "http://localhost:5000/12.jpg", Type = "Medium"},
                new Vehicle {Manufacturer = "Toyota", Model = "Yaris", AverageGrade = 8.6, Year = 2018, Doors = 4, Seats = 4, Price = 120, Photo = "http://localhost:5000/13.jpg", Type = "Small"},
                new Vehicle {Manufacturer = "Chevrolet", Model = "Camaro", AverageGrade = 9.6, Year = 2017, Doors = 2, Seats = 5, Price = 145, Photo = "http://localhost:5000/9.jpg", Type = "Small"},
                new Vehicle {Manufacturer = "Ford", Model = "Fiesta", AverageGrade = 9.4,  Year = 2015, Doors = 4, Seats = 5, Price = 214, Photo = "http://localhost:5000/14.jpg", Type = "Medium"},
                new Vehicle {Manufacturer = "Nissan", Model = "Versa", AverageGrade = 8.6,  Year = 2020, Doors = 4, Seats = 5, Price = 146, Photo = "http://localhost:5000/15.png", Type = "Medium"},
                new Vehicle {Manufacturer = "Kia", Model = "Rio", AverageGrade = 8.3,  Year = 2018, Doors = 4, Seats = 5, Price = 210, Photo = "http://localhost:5000/16.jpg", Type = "Small"},
                new Vehicle {Manufacturer = "Mitsubishi", Model = "Mirage", AverageGrade = 8.2, Year = 2017, Doors = 4, Seats = 4, Price = 365, Photo = "http://localhost:5000/17.jpg", Type = "Medium, Luxury"},
            };

            return vehicles;
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

