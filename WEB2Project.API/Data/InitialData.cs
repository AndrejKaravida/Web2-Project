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

                if(context.Reservations.Any())
                     CheckReservations(context);

                if (context.AirCompanies.Any())
                    return;

                var vehicleRatings = GetVehicleRatings().ToArray();
                context.VehicleRatings.AddRange(vehicleRatings);
                context.SaveChanges();

                var companyRatings = GetCompanyRatings().ToArray();
                context.CompanyRatings.AddRange(companyRatings);
                context.SaveChanges();

                var locations = GetLocations().ToArray();
                context.Locations.AddRange(locations);
                context.SaveChanges();

                var vehicles = GetVehicles(context).ToArray();
                context.Vehicles.AddRange(vehicles);
                context.SaveChanges();

                var discountedVehicles = GetDiscountedVehicles(context).ToArray();
                context.VehiclesOnDiscount.AddRange(discountedVehicles);
                context.SaveChanges();

                var destinations = GetDestinations().ToArray();
                context.Destinations.AddRange(destinations);
                context.SaveChanges();

                var flights = GetFlights(context).ToArray();
                context.Flights.AddRange(flights);
                context.SaveChanges();

                var aircompanies = GetAirCompanies(context).ToArray();
                context.AirCompanies.AddRange(aircompanies);
                context.SaveChanges();

                var rentacarcompanies = GetRentACarCompanies(context).ToArray();
                context.RentACarCompanies.AddRange(rentacarcompanies);
                context.SaveChanges();
            }
        }

        public static void CheckReservations(DataContext db)
        {
            var reservations = db.Reservations.ToList();
            foreach (var r in reservations)
            {
                if(r.EndDate.Date < DateTime.Now)
                {
                    r.Vehicle.IsReserved = false;
                }
            }
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
                new RentACarCompany {Name = "Alamo rentals", Address = "Brussel, Belgium", AverageGrade = 9.1,
                    Photo="http://localhost:5000/alamocompany.png",
                    WeekRentalDiscount = 10, MonthRentalDiscount = 19, Incomes = new List<Income>(), PromoDescription = "The best Rental in town!", 
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Take(10)), VehiclesOnDiscount = new List<VehicleOnDiscount>(db.VehiclesOnDiscount.Take(2)),
                    Locations = new List<Location>{locations[0], locations[1]}, Vehicles = new List<Vehicle>(db.Vehicles.Take(3))},
              
                new RentACarCompany {Name = "Hertz rentals", Address = "Roma, Italy", AverageGrade = 9.4,
                    WeekRentalDiscount = 15, MonthRentalDiscount = 26, Incomes = new List<Income>(), PromoDescription = "Drive with professionals",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(10).Take(10)),
                    Photo="http://localhost:5000/hertzcompany.png", VehiclesOnDiscount = new List<VehicleOnDiscount>(db.VehiclesOnDiscount.Skip(2).Take(2)),
                    Locations = new List<Location>{locations[2], locations[3]}, Vehicles = new List<Vehicle>(db.Vehicles.Skip(3).Take(3))},
               
                new RentACarCompany {Name = "Enterprise rentals", Address = "Ankara, Turkey", AverageGrade = 9.1,
                    WeekRentalDiscount = 12, MonthRentalDiscount = 24, Incomes = new List<Income>(), PromoDescription = "Experience is in our name",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(20).Take(10)),
                    Photo="http://localhost:5000/enterprisecompany.png", VehiclesOnDiscount = new List<VehicleOnDiscount>(db.VehiclesOnDiscount.Skip(4).Take(2)),
                    Locations = new List<Location>{locations[4], locations[5]}, Vehicles = new List<Vehicle>(db.Vehicles.Skip(6).Take(3))},
              
                new RentACarCompany {Name = "Turo rentals", Address = "San Francisco, California", AverageGrade = 8.1,
                    WeekRentalDiscount = 14, MonthRentalDiscount = 22, Incomes = new List<Income>(), PromoDescription = "Dedicated to car rentals",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(30).Take(10)),
                    Photo="http://localhost:5000/turo.jpg", VehiclesOnDiscount = new List<VehicleOnDiscount>(db.VehiclesOnDiscount.Skip(6).Take(2)),
                    Locations = new List<Location>{locations[6], locations[7]}, Vehicles = new List<Vehicle>(db.Vehicles.Skip(9).Take(3)) },
             
                new RentACarCompany {Name = "Europcar rentals", Address = "Paris, France", AverageGrade = 9.4,
                    WeekRentalDiscount = 8, MonthRentalDiscount = 18, Incomes = new List<Income>(), PromoDescription = "Moving your way",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(40).Take(10)),
                    Photo="http://localhost:5000/europcar.png", VehiclesOnDiscount = new List<VehicleOnDiscount>(db.VehiclesOnDiscount.Skip(8).Take(2)),
                    Locations = new List<Location>{locations[8], locations[9]}, Vehicles = new List<Vehicle>(db.Vehicles.Skip(12).Take(4))}
            };

            return rentACarCompanies;
        }

        public static List<Location> GetLocations()
        {
            List<Location> locations = new List<Location>()
            { 
                new Location {Address = "Thermae 156/18, Brussel", Country = "Belgium"},
                new Location {Address = "Cascata delle Superiore 4, Roma", Country = "Italy"},
                new Location {Address = "Birlik Mosque 59, Ankara", Country = "Turkey"},
                new Location {Address = "Hühnermoor 18, Hamburg", Country = "Germany"},
                new Location {Address = "Elbterrassen Brambach 134, Leipzig", Country = "Germany"},
                new Location {Address = "Castillo de Pedraza 12, Madrid", Country = "Spain "},
                new Location {Address = "Aux Délices 14, Nantes", Country = "France"},
                new Location {Address = "Wolfengasse 3, Vienna", Country = "Austria"},
                new Location {Address = "Turfmarkt 147, Amesterdam", Country = "Netherlands"},
                new Location {Address = "Zmaj Jovina 10, Novi Sad", Country = "Serbia"}
            };

            return locations;
        }

        public static List<AirCompany> GetAirCompanies(DataContext db)
        {
            List<AirCompany> airCompanies = new List<AirCompany>()
            {
                new AirCompany {Name = "Qatar Airways", Address="Doha, Qatar", AverageGrade = 10,
                    Photo = "http://localhost:5000/qatar.png", Flights = new List<Flight>(db.Flights.Take(200)),
                    PromoDescription = "We are in this together"},
               
                new AirCompany {Name = "Singapore Airlines", Address="Singapore, Singapore", AverageGrade = 9.2,
                    Photo = "http://localhost:5000/singapore.png", Flights = new List<Flight>(db.Flights.Skip(200).Take(200)),
                    PromoDescription = "Enjoy world-class service"},
           
                new AirCompany {Name = "Emirates", Address="Dubai, UAE", AverageGrade = 8.9,
                    Photo = "http://localhost:5000/emirates.png", Flights = new List<Flight>(db.Flights.Skip(400).Take(200)),
                    PromoDescription = "Choose Emirates airline to enjoy our world-class service on all flights"},
             
                new AirCompany {Name = "Lufthansa", Address="Koln, Germany", AverageGrade = 8.4,
                    Photo = "http://localhost:5000/lufthansa.png", Flights = new List<Flight>(db.Flights.Skip(600).Take(200)),
                    PromoDescription = "The Lufthansa Group is an aviation group with operations worldwide"},
             
                new AirCompany {Name = "Air Serbia", Address="Belgrade, Serbia", AverageGrade = 7.6,
                    Photo = "http://localhost:5000/serbia.png", Flights = new List<Flight>(db.Flights.Skip(800).Take(200)),
                    PromoDescription = "Air Serbia has been a leader in air transport since the company was founded in 1927"}
            };

            return airCompanies;
        }

        public static List<Vehicle> GetVehicles(DataContext db)
        {
            List<Vehicle> vehicles = new List<Vehicle>()
            {
                new Vehicle {Manufacturer = "Alfa Romeo", Model = "Giulia", AverageGrade = 8.6, Year = 2016, Doors = 4, Seats = 5, Price = 369, IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/1.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Take(10))},
                new Vehicle {Manufacturer = "Alfa Romeo", Model = "Quadrifoglio", AverageGrade = 8.8, Year = 2020 , Doors = 4, Seats = 5, Price = 158, IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/2.jpg" , Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(10).Take(10))},
                new Vehicle {Manufacturer = "Audi", Model = "A5 Sportback", AverageGrade = 9.6, Year = 2018, Doors = 2, Seats = 2, Price = 347,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/3.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(20).Take(10))},
                new Vehicle {Manufacturer = "Audi", Model = "A6", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, Price = 395,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/4.jpg", Type = "Large",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(30).Take(10))},
                new Vehicle {Manufacturer = "Audi", Model = "A7", AverageGrade = 8.6,  Year = 2016, Doors = 4, Seats = 5, Price = 390,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/5.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(40).Take(10))},
                new Vehicle {Manufacturer = "Audi", Model = "A8", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, Price = 399,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/6.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(50).Take(10))},
                new Vehicle {Manufacturer = "Genesis", Model = "G70", AverageGrade = 7.9,  Year = 2018, Doors = 4, Seats = 5, Price = 260,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/7.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(60).Take(10))},
                new Vehicle {Manufacturer = "BMW ", Model = "2-series", AverageGrade = 6.6, Year = 2015, Doors = 2, Seats = 5, Price = 290,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/8.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(70).Take(10))},
                new Vehicle {Manufacturer = "Chevrolet", Model = "Corvette", AverageGrade = 7.9,  Year = 2016, Doors = 2, Seats = 5, Price = 390,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/10.jpg", Type = "Small", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(80).Take(10))},
                new Vehicle {Manufacturer = "Ford", Model = "Mustang", AverageGrade = 8.6, Year = 2014, Doors = 4, Seats = 5, Price = 380,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/11.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(90).Take(10))},
                new Vehicle {Manufacturer = "Honda", Model = "Accord", AverageGrade = 7.6, Year = 2016, Doors = 4, Seats = 5, Price = 130,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/12.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(100).Take(10))},
                new Vehicle {Manufacturer = "Toyota", Model = "Yaris", AverageGrade = 8.6, Year = 2018, Doors = 4, Seats = 4, Price = 120,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/13.jpg", Type = "Small",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(110).Take(10))},
                new Vehicle {Manufacturer = "Chevrolet", Model = "Camaro", AverageGrade = 9.6, Year = 2017, Doors = 2, Seats = 5, Price = 145,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/9.jpg", Type = "Small", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(120).Take(10))},
                new Vehicle {Manufacturer = "Ford", Model = "Fiesta", AverageGrade = 9.4,  Year = 2015, Doors = 4, Seats = 5, Price = 214,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/14.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(130).Take(10))},
                new Vehicle {Manufacturer = "Nissan", Model = "Versa", AverageGrade = 8.6,  Year = 2020, Doors = 4, Seats = 5, Price = 146,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/15.png", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(140).Take(10))},
                new Vehicle {Manufacturer = "Kia", Model = "Rio", AverageGrade = 8.3,  Year = 2018, Doors = 4, Seats = 5, Price = 210,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/16.jpg", Type = "Small", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(150).Take(10))},
                new Vehicle {Manufacturer = "Mitsubishi", Model = "Mirage", AverageGrade = 8.2, Year = 2017, Doors = 4, Seats = 4, Price = 365,  IsDeleted = false, IsReserved = false,
                    Photo = "http://localhost:5000/17.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(160).Take(10))},
            };

            return vehicles;
        }

        public static List<VehicleOnDiscount> GetDiscountedVehicles (DataContext db)
        {
            List<VehicleOnDiscount> discountedVehicles = new List<VehicleOnDiscount>()
            {
                new VehicleOnDiscount {Manufacturer = "Hyundai", Model = "Veloster N", AverageGrade = 8.6, Year = 2016, Doors = 4, Seats = 5, OldPrice = 369,
                    IsDeleted = false, IsReserved = false, NewPrice = 296,
                    Photo = "http://localhost:5000/18.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(170).Take(10))},

                new VehicleOnDiscount {Manufacturer = "Mazda", Model = "3", AverageGrade = 8.8, Year = 2020 , Doors = 4, Seats = 5, OldPrice = 348, 
                    IsDeleted = false, IsReserved = false, NewPrice = 289,
                    Photo = "http://localhost:5000/19.jpg" , Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(180).Take(10))},

                new VehicleOnDiscount {Manufacturer = "Mercedes", Model = "AMG C43 Sedan", AverageGrade = 9.6, Year = 2018, Doors = 4, Seats = 5, OldPrice = 347, 
                    IsDeleted = false, IsReserved = false, NewPrice = 296,
                    Photo = "http://localhost:5000/20.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(190).Take(10))},
               
                new VehicleOnDiscount {Manufacturer = "Mercedes", Model = "AMG E53 coupe", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, OldPrice = 395,  
                    IsDeleted = false, IsReserved = false, NewPrice = 268,
                    Photo = "http://localhost:5000/21.jpg", Type = "Large",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(200).Take(10))},
              
                new VehicleOnDiscount {Manufacturer = "Porsche", Model = "911 GT3 RS", AverageGrade = 8.6,  Year = 2016, Doors = 2, Seats = 3, OldPrice = 400, 
                    IsDeleted = false, IsReserved = false, NewPrice = 310,
                    Photo = "http://localhost:5000/22.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(210).Take(10))},
              
                new VehicleOnDiscount {Manufacturer = "Volkswagen", Model = "Golf GTI", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, OldPrice = 210, 
                    IsDeleted = false, IsReserved = false, NewPrice = 148,
                    Photo = "http://localhost:5000/23.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(220).Take(10))},
             
                new VehicleOnDiscount {Manufacturer = "Volkswagen", Model = "Golf SportWagen", AverageGrade = 7.9,  Year = 2018, Doors = 4, Seats = 5, OldPrice = 260, 
                    IsDeleted = false, IsReserved = false, NewPrice = 180,
                    Photo = "http://localhost:5000/24.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(230).Take(10))},
             
                new VehicleOnDiscount {Manufacturer = "Volvo", Model = "V90", AverageGrade = 6.6, Year = 2015, Doors = 4, Seats = 5, OldPrice = 320,  
                    IsDeleted = false, IsReserved = false, NewPrice = 270,
                    Photo = "http://localhost:5000/25.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(240).Take(10))},
             
                new VehicleOnDiscount {Manufacturer = "Volvo", Model = "V90 Cross Country", AverageGrade = 7.9,  Year = 2016, Doors = 4, Seats = 5, OldPrice = 360, 
                    IsDeleted = false, IsReserved = false, NewPrice = 275,
                    Photo = "http://localhost:5000/26.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(250).Take(10))},
            
                new VehicleOnDiscount {Manufacturer = "Porsche", Model = "911 Turbo/Turbo S", AverageGrade = 8.6, Year = 2014, Doors = 2, Seats = 3, OldPrice = 380,  
                    IsDeleted = false, IsReserved = false, NewPrice = 310,
                    Photo = "http://localhost:5000/27.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(260).Take(10))}       
            };

            return discountedVehicles;
        }

        public static List<Flight> GetFlights(DataContext db)
        {
            Random random = new Random();

            List<Flight> flights = new List<Flight>();

            for (int i = 0; i < 1010; i++)
            {
                var departureTime = DateTime.Now.AddDays(random.Next(1, 15)).AddHours(random.Next(1, 14)).AddMinutes(random.Next(1, 59)); 
                var arrivalTime = departureTime.AddHours(random.Next(1, 3)).AddMinutes(random.Next(1, 59));
                var ticketPrice = random.Next(100, 550);
                var mileage = random.Next(100, 1500);
                var avgGrade = random.Next(6, 10);
                var travelTime = (arrivalTime - departureTime).TotalMinutes;

                Flight flight = new Flight {
                    DepartureDestination = db.Destinations.Skip(random.Next(1, 9)).First(), 
                    ArrivalDestination = db.Destinations.Skip(random.Next(1, 9)).First(), 
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    TravelTime = travelTime, 
                    AverageGrade = avgGrade,
                    TicketPrice = ticketPrice,  
                    Mileage = mileage
                };

                if(flight.DepartureDestination.City != flight.ArrivalDestination.City)
                {
                    flights.Add(flight);
                }
            }

            return flights;
        }

        public static List<VehicleRating> GetVehicleRatings()
        {
            Random r = new Random();

            List<VehicleRating> ratings = new List<VehicleRating>();

            for(int i = 0; i < 275; i++)
            {
                var value = r.Next(5, 11);

                VehicleRating rating = new VehicleRating() { Value = value };
                ratings.Add(rating);
            }

            return ratings;
        }

        public static List<CompanyRating> GetCompanyRatings()
        {
            Random r = new Random();

            List<CompanyRating> ratings = new List<CompanyRating>();

            for (int i = 0; i < 55; i++)
            {
                var value = r.Next(5, 11);

                CompanyRating rating = new CompanyRating() { Value = value };
                ratings.Add(rating);
            }

            return ratings;
        }
    }


}

