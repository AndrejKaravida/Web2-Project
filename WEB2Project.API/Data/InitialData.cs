using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using WEB2Project.API.Data;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Data
{
    public class InitialData
    {   
        public static void Initialize(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<DataContext>();
                context.Database.EnsureCreated();

                if (context.AirCompanies.Any())
                    return;

                var vehicleRatings = GetVehicleRatings().ToArray();
                context.VehicleRatings.AddRange(vehicleRatings);
                context.SaveChanges();

                var companyRatings = GetCompanyRatings().ToArray();
                context.CompanyRatings.AddRange(companyRatings);
                context.SaveChanges();

                var vehicles = GetVehicles(context).ToArray();
                context.Vehicles.AddRange(vehicles);
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

                LoadFirstDestinations(context);
                LoadHeadOffice(context);
            }
        }

        public static void LoadFirstDestinations(DataContext db)
        {
             Random r = new Random();

            var companies = db.RentACarCompanies
                .Include(d => d.Destinations)
                .Include(v => v.Vehicles)
                .ToList();

            foreach(var company in companies)
            {
                foreach(var v in company.Vehicles)
                {
                    
                    v.CurrentDestination = company.Destinations.Skip(r.Next(0,2)).FirstOrDefault().City;
                }
            }

            db.SaveChanges();
        }

        public static void LoadHeadOffice(DataContext db)
        {
            var companies = db.RentACarCompanies
                .Include(h => h.HeadOffice)
                .Include(d => d.Destinations)
                .ToList();

            foreach (var company in companies)
            {
                company.HeadOffice = company.Destinations.FirstOrDefault();
            }
            db.SaveChanges();
        }
 
        public static List<Destination> GetDestinations()
        {
            List<Destination> destinations = new List<Destination>()
            { 
                new Destination {City = "Budapest", Country = "Hungary", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1pl2eHo_WwW_X6Zf-KqjqelJxblCPYjP3"},
                new Destination {City = "Belgrade", Country = "Serbia", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1zg_rjnHGYDq_REwz8SR6yQaqL3nK7iW1"},
                new Destination {City = "Milan", Country = "Italy", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1aikVlNSmN-MiDPZeBlh2p_wAkvjENPv0"},
                new Destination {City = "Vienna", Country = "Austria", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1g-2SPMn-WV84TFp-wLdhtW547lzhG4CJ"},
                new Destination {City = "Malmo", Country = "Sweden", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1g1qjAzrFqAEzeG1ctjebzw2ouuQdH5MV"},
                new Destination {City = "Berlin", Country = "Germany", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1FYL0JOESlGFpoZ-2bNn23Qg4A3_JF38P"},
                new Destination {City = "Las Vegas", Country = "USA", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1opcXpVUk1o7IB7WIEnKqZihTwBBC08jw"},
                new Destination {City = "Frankfurt", Country = "Germany", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1_9yoifuChe54r5mT31qxk9Hns_i3slcx"},
                new Destination {City = "Paris", Country = "France", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1vc8fND63j3Tq_0ENGc75YUUJsSuSY2Wl"},
                new Destination {City = "Moscow", Country = "Russia", MapString ="https://www.google.com/maps/d/u/0/embed?mid=1laDN_OPD3WBjDHshjH3jm3mhygs1aD-j"}
            };

            return destinations;
        }

        public static List<RentACarCompany> GetRentACarCompanies(DataContext db)
        {
            List<RentACarCompany> rentACarCompanies = new List<RentACarCompany>()
            {
                new RentACarCompany {Name = "Alamo rentals", AverageGrade = 9.1, 
                    Photo="http://localhost:5000/alamocompany.png",
                    WeekRentalDiscount = 10, MonthRentalDiscount = 19, 
                    Incomes = new List<Income>(), PromoDescription = "The best Rental in town!", 
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Take(10)),
                    Destinations = new List<Destination>(db.Destinations.Take(2)),
                    Vehicles = new List<Vehicle>(db.Vehicles.Take(14))},
              
                new RentACarCompany {Name = "Hertz rentals", AverageGrade = 9.4, 
                    Photo="http://localhost:5000/hertzcompany.png",
                    WeekRentalDiscount = 15, MonthRentalDiscount = 26, 
                    Incomes = new List<Income>(), PromoDescription = "Drive with professionals",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(10).Take(10)),
                    Destinations = new List<Destination>(db.Destinations.Skip(2).Take(2)), 
                    Vehicles = new List<Vehicle>(db.Vehicles.Skip(14).Take(14))},
               
                new RentACarCompany {Name = "Enterprise rentals", AverageGrade = 9.1,
                    WeekRentalDiscount = 12, MonthRentalDiscount = 24, 
                    Incomes = new List<Income>(), PromoDescription = "Experience is in our name",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(20).Take(10)),
                    Photo="http://localhost:5000/enterprisecompany.png",
                    Destinations = new List<Destination>(db.Destinations.Skip(4).Take(2)), 
                    Vehicles = new List<Vehicle>(db.Vehicles.Skip(28).Take(14))},
              
                new RentACarCompany {Name = "Turo rentals", AverageGrade = 8.1,
                    WeekRentalDiscount = 14, MonthRentalDiscount = 22, 
                    Incomes = new List<Income>(), PromoDescription = "Dedicated to car rentals",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(30).Take(10)),
                    Photo="http://localhost:5000/turo.png",
                    Destinations = new List<Destination>(db.Destinations.Skip(6).Take(2)),
                    Vehicles = new List<Vehicle>(db.Vehicles.Skip(42).Take(14)) },
             
                new RentACarCompany {Name = "Europcar rentals", AverageGrade = 9.4,
                    WeekRentalDiscount = 8, MonthRentalDiscount = 18, 
                    Incomes = new List<Income>(), PromoDescription = "Moving your way",
                    Ratings = new List<CompanyRating>(db.CompanyRatings.Skip(40).Take(10)),
                    Photo="http://localhost:5000/europcar.png",
                    Destinations = new List<Destination>(db.Destinations.Skip(8).Take(2)), 
                    Vehicles = new List<Vehicle>(db.Vehicles.Skip(56).Take(14))}
            };

            return rentACarCompanies;
        }

        public static List<AirCompany> GetAirCompanies(DataContext db)
        {
            List<AirCompany> airCompanies = new List<AirCompany>()
            {
                new AirCompany {Name = "Qatar Airways", HeadOffice=db.Destinations.Skip(7).First(), AverageGrade = 10,
                    Photo = "http://localhost:5000/qatar.png", Flights = new List<Flight>(db.Flights.Take(100)),
                    PromoDescription = "We are in this together"},
               
                new AirCompany {Name = "Singapore Airlines", HeadOffice=db.Destinations.Skip(5).First(), AverageGrade = 9.2,
                    Photo = "http://localhost:5000/singapore.png", Flights = new List<Flight>(db.Flights.Skip(100).Take(100)),
                    PromoDescription = "Enjoy world-class service"},
           
                new AirCompany {Name = "Emirates", HeadOffice=db.Destinations.Skip(4).First(), AverageGrade = 8.9,
                    Photo = "http://localhost:5000/emirates.png", Flights = new List<Flight>(db.Flights.Skip(200).Take(100)),
                    PromoDescription = "Choose Emirates airline to enjoy our world-class service on all flights"},
             
                new AirCompany {Name = "Lufthansa", HeadOffice=db.Destinations.Skip(2).First(), AverageGrade = 8.4,
                    Photo = "http://localhost:5000/lufthansa.png", Flights = new List<Flight>(db.Flights.Skip(300).Take(100)),
                    PromoDescription = "The Lufthansa Group is an aviation group with operations worldwide"},
             
                new AirCompany {Name = "Air Serbia", HeadOffice=db.Destinations.Skip(8).First(), AverageGrade = 7.6,
                    Photo = "http://localhost:5000/serbia.png", Flights = new List<Flight>(db.Flights.Skip(400).Take(100)),
                    PromoDescription = "Air Serbia has been a leader in air transport since the company was founded in 1927"}
            };

            return airCompanies;
        }

        public static List<Vehicle> GetVehicles(DataContext db)
        {
            List<Vehicle> vehicles = new List<Vehicle>()
            {

                // ---- COMPANY 1 -----

                new Vehicle {Manufacturer = "Genesis", Model = "G70", AverageGrade = 7.9,  Year = 2018, Doors = 4, Seats = 5, Price = 260,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/7.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Take(5))},

                new Vehicle {Manufacturer = "Alfa Romeo", Model = "Giulia", AverageGrade = 8.6, Year = 2016, Doors = 4, Seats = 5, Price = 369, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/1.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(5).Take(5))},
              
                new Vehicle {Manufacturer = "Alfa Romeo", Model = "Quadrifoglio", AverageGrade = 8.8, Year = 2020 , Doors = 4, Seats = 5, Price = 158, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/2.jpg" , Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(10).Take(5))},
             
                new Vehicle {Manufacturer = "Audi", Model = "A5 Sportback", AverageGrade = 9.6, Year = 2018, Doors = 2, Seats = 2, Price = 347,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/3.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(15).Take(5))},
               
                new Vehicle {Manufacturer = "BMW", Model = "M5", AverageGrade = 9.5, Year = 2019, Doors = 4, Seats = 4, Price = 369,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/28.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(20).Take(5))},
                
                new Vehicle {Manufacturer = "BMW", Model = "M2 Competition", AverageGrade = 8.4, Year = 2017, Doors = 2, Seats = 5, Price = 318,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/29.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(25).Take(5))},

                new Vehicle {Manufacturer = "Chevrolet", Model = "Corvette Z06", AverageGrade = 8.7, Year = 2016, Doors = 2, Seats = 2, Price = 313,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/30.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(30).Take(5))},
                  
                new Vehicle {Manufacturer = "Audi", Model = "A6", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, Price = 395,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/4.jpg", Type = "Large",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(35).Take(5))},

                new Vehicle {Manufacturer = "Audi", Model = "A7", AverageGrade = 8.6,  Year = 2016, Doors = 4, Seats = 5, Price = 390,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/5.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(40).Take(5))},

                new Vehicle {Manufacturer = "Hyundai", Model = "Veloster N", AverageGrade = 8.6, Year = 2016, Doors = 4, Seats = 5, Price = 296,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/18.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(45).Take(5))},

                new Vehicle {Manufacturer = "Mazda", Model = "3", AverageGrade = 8.8, Year = 2020 , Doors = 4, Seats = 5, Price = 289,
                    IsDeleted = false, OldPrice = 348, IsOnDiscount = true,
                    Photo = "http://localhost:5000/19.jpg" , Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(50).Take(5))},
               
                new Vehicle {Manufacturer = "Mercedes", Model = "AMG E53 coupe", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, Price = 268,
                    IsDeleted = false, OldPrice = 395, IsOnDiscount = true,
                    Photo = "http://localhost:5000/21.jpg", Type = "Large",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(55).Take(5))},
                
                new Vehicle {Manufacturer = "Volkswagen", Model = "Golf GTI", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, Price = 148,
                    IsDeleted = false, OldPrice = 210, IsOnDiscount = true,
                    Photo = "http://localhost:5000/23.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(60).Take(5))},
                
                new Vehicle {Manufacturer = "Volvo", Model = "V90", AverageGrade = 6.6, Year = 2015, Doors = 4, Seats = 5, Price = 270,
                    IsDeleted = false, OldPrice = 320, IsOnDiscount = true,
                    Photo = "http://localhost:5000/25.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(65).Take(5))},



                // ---- COMPANY 2 -----

                new Vehicle {Manufacturer = "Audi", Model = "A6", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, Price = 395, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/4.jpg", Type = "Large",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(70).Take(5))},
              
                new Vehicle {Manufacturer = "Audi", Model = "A7", AverageGrade = 8.6,  Year = 2016, Doors = 4, Seats = 5, Price = 390, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/5.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(75).Take(5))},
              
                new Vehicle {Manufacturer = "Audi", Model = "A8", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, Price = 399, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/6.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(80).Take(5))},
               
                new Vehicle {Manufacturer = "Chevrolet", Model = "Corvette ZR1", AverageGrade = 9.6, Year = 2020, Doors = 2, Seats = 4, Price = 380,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/31.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(85).Take(5))},
               
                new Vehicle {Manufacturer = "Chevrolet", Model = "Spark", AverageGrade = 7.6, Year = 2015, Doors = 2, Seats = 5, Price = 230,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/32.jpg", Type = "Small", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(90).Take(5))},
                 
                new Vehicle {Manufacturer = "Chevrolet", Model = "Volt", AverageGrade = 8.9, Year = 2018, Doors = 4, Seats = 5, Price = 260,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/33.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(95).Take(5))},
                
                new Vehicle {Manufacturer = "Volkswagen", Model = "Golf GTI", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, Price = 148,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/23.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(100).Take(5))},

                new Vehicle {Manufacturer = "Volvo", Model = "V90", AverageGrade = 6.6, Year = 2015, Doors = 4, Seats = 5, Price = 270,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/25.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(105).Take(5))},
                
                new Vehicle {Manufacturer = "Ford", Model = "Mustang Shelby GT350", AverageGrade = 8.6, Year = 2016, Doors = 2, Seats = 2, Price = 285,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/34.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(110).Take(5))},
                
                new Vehicle {Manufacturer = "Mitsubishi", Model = "Mirage", AverageGrade = 8.2, Year = 2017, Doors = 4, Seats = 4, Price = 365,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/17.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(115).Take(5))},



                new Vehicle {Manufacturer = "Mercedes", Model = "AMG C43 Sedan", AverageGrade = 9.6, Year = 2018, Doors = 4, Seats = 5, Price = 296,
                    IsDeleted = false, OldPrice = 347, IsOnDiscount = true,
                    Photo = "http://localhost:5000/20.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(120).Take(5))},

                new Vehicle {Manufacturer = "Mercedes", Model = "AMG E53 coupe", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, Price = 268,
                    IsDeleted = false, OldPrice = 395, IsOnDiscount = true,
                    Photo = "http://localhost:5000/21.jpg", Type = "Large",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(125).Take(5))},

                new Vehicle {Manufacturer = "Honda", Model = "Accord", AverageGrade = 7.6, Year = 2016, Doors = 4, Seats = 5, Price = 130,
                    IsDeleted = false, OldPrice = 185, IsOnDiscount = true,
                    Photo = "http://localhost:5000/12.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(130).Take(5))},

                new Vehicle {Manufacturer = "Toyota", Model = "Yaris", AverageGrade = 8.6, Year = 2018, Doors = 4, Seats = 4, Price = 120,
                    IsDeleted = false, OldPrice = 179,  IsOnDiscount = true,
                    Photo = "http://localhost:5000/13.jpg", Type = "Small",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(135).Take(5))},





                // ---- COMPANY 3 -----


                new Vehicle {Manufacturer = "Genesis", Model = "G70", AverageGrade = 7.9,  Year = 2018, Doors = 4, Seats = 5, Price = 260,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/7.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(140).Take(5))},
               
                new Vehicle {Manufacturer = "BMW ", Model = "2-series", AverageGrade = 6.6, Year = 2015, Doors = 2, Seats = 5, Price = 290, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/8.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(145).Take(5))},
              
                new Vehicle {Manufacturer = "Chevrolet", Model = "Corvette", AverageGrade = 7.9,  Year = 2016, Doors = 2, Seats = 5, Price = 390, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/10.jpg", Type = "Small", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(150).Take(5))},
              
                new Vehicle {Manufacturer = "Ford", Model = "Mustang", AverageGrade = 8.6, Year = 2014, Doors = 4, Seats = 5, Price = 380, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/11.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(155).Take(5))},
               
                new Vehicle {Manufacturer = "Honda", Model = "Accord Hybrid", AverageGrade = 8.2, Year = 2018, Doors = 4, Seats = 5, Price = 290,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/35.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(160).Take(5))},
               
                new Vehicle {Manufacturer = "Honda", Model = "Civic Sedan", AverageGrade = 8.4, Year = 2017, Doors = 4, Seats = 5, Price = 270,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/17.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(165).Take(5))},

                new Vehicle {Manufacturer = "Honda", Model = "Civic Hatchback", AverageGrade = 8.5, Year = 2016, Doors = 4, Seats = 4, Price = 220,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/37.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(170).Take(5))},
               
                new Vehicle {Manufacturer = "Audi", Model = "A7", AverageGrade = 8.6,  Year = 2018, Doors = 4, Seats = 5, Price = 390,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/5.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(175).Take(5))},

                new Vehicle {Manufacturer = "Hyundai", Model = "Veloster N", AverageGrade = 8.6, Year = 2016, Doors = 4, Seats = 5, Price = 296,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/18.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(180).Take(5))},

                new Vehicle {Manufacturer = "Honda", Model = "Insight", AverageGrade = 7.9, Year = 2017, Doors = 4, Seats = 5, Price = 231,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/38.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(185).Take(5))},

                new Vehicle {Manufacturer = "Porsche", Model = "911 GT3 RS", AverageGrade = 8.6,  Year = 2016, Doors = 2, Seats = 3, Price = 310,
                    IsDeleted = false, OldPrice = 400, IsOnDiscount = true,
                    Photo = "http://localhost:5000/22.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(190).Take(5))},

                new Vehicle {Manufacturer = "Volkswagen", Model = "Golf GTI", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, Price = 148,
                    IsDeleted = false, OldPrice = 210, IsOnDiscount = true,
                    Photo = "http://localhost:5000/23.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(195).Take(5))},

                new Vehicle {Manufacturer = "Hyundai", Model = "Ioniq", AverageGrade = 6.6, Year = 2014, Doors = 4, Seats = 5, Price = 138,
                    IsDeleted = false, IsOnDiscount = true, OldPrice = 190,
                    Photo = "http://localhost:5000/18.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(200).Take(5))},
               
                new Vehicle {Manufacturer = "Kia", Model = "Cadenza", AverageGrade = 9.6, Year = 2019, Doors = 4, Seats = 5, Price = 260,
                    IsDeleted = false, IsOnDiscount = true, OldPrice = 360,
                    Photo = "http://localhost:5000/39.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(205).Take(5))},



                // ---- COMPANY 4 -----


                new Vehicle {Manufacturer = "Honda", Model = "Accord", AverageGrade = 7.6, Year = 2016, Doors = 4, Seats = 5, Price = 130, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/12.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(210).Take(5))},
              
                new Vehicle {Manufacturer = "Toyota", Model = "Yaris", AverageGrade = 8.6, Year = 2018, Doors = 4, Seats = 4, Price = 120, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/13.jpg", Type = "Small",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(215).Take(5))},
               
                new Vehicle {Manufacturer = "Chevrolet", Model = "Camaro", AverageGrade = 9.6, Year = 2017, Doors = 2, Seats = 5, Price = 145, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/9.jpg", Type = "Small", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(220).Take(5))},
               
                new Vehicle {Manufacturer = "Ford", Model = "Fiesta", AverageGrade = 9.4,  Year = 2015, Doors = 4, Seats = 5, Price = 214, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/14.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(225).Take(5))},
               
                new Vehicle {Manufacturer = "Kia", Model = "Rio Hatchback", AverageGrade = 8.4,  Year = 2015, Doors = 4, Seats = 5, Price = 225,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/40.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(230).Take(5))},
                
                new Vehicle {Manufacturer = "Kia", Model = "Stinger", AverageGrade = 8.9,  Year = 2019, Doors = 4, Seats = 5, Price = 245,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/41.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(235).Take(5))},
                
                new Vehicle {Manufacturer = "Mazda", Model = "MX-5 Miata", AverageGrade = 8.6,  Year = 2019, Doors = 2, Seats = 2, Price = 289,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/42.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(240).Take(5))},
               
                new Vehicle {Manufacturer = "McLaren", Model = "570S GT", AverageGrade = 9.9,  Year = 2020, Doors = 2, Seats = 2, Price = 400,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/43.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(245).Take(5))},
               
                new Vehicle {Manufacturer = "Mercedes", Model = "AMG C63 cabriolet", AverageGrade = 8.9,  Year = 2019, Doors = 4, Seats = 5, Price = 296,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/44.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(250).Take(5))},
                
                new Vehicle {Manufacturer = "Mercedes", Model = "AMG E63", AverageGrade = 8.6,  Year = 2019, Doors = 4, Seats = 5, Price = 330,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/45.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(255).Take(5))},

                new Vehicle {Manufacturer = "Volkswagen", Model = "Golf SportWagen", AverageGrade = 7.9,  Year = 2018, Doors = 4, Seats = 5, Price = 180,
                    IsDeleted = false, OldPrice = 260, IsOnDiscount = true,
                    Photo = "http://localhost:5000/24.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(260).Take(5))},

                new Vehicle {Manufacturer = "Volvo", Model = "V90", AverageGrade = 6.6, Year = 2015, Doors = 4, Seats = 5, Price = 270,
                    IsDeleted = false, OldPrice = 320, IsOnDiscount = true,
                    Photo = "http://localhost:5000/25.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(265).Take(5))},
                
                new Vehicle {Manufacturer = "Mercedes", Model = "AMG E63 S Wagon", AverageGrade = 7.6, Year = 2016, Doors = 4, Seats = 5, Price = 260,
                    IsDeleted = false, OldPrice = 330, IsOnDiscount = true,
                    Photo = "http://localhost:5000/46.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(270).Take(5))},
               
                new Vehicle {Manufacturer = "Mercedes", Model = "AMG S63/S65", AverageGrade = 9.3, Year = 2019, Doors = 4, Seats = 5, Price = 290,
                    IsDeleted = false, OldPrice = 350, IsOnDiscount = true,
                    Photo = "http://localhost:5000/27.jpg", Type = "Luxury",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(275).Take(5))},


                // ---- COMPANY 5 -----

                new Vehicle {Manufacturer = "Nissan", Model = "Versa", AverageGrade = 8.6,  Year = 2020, Doors = 4, Seats = 5, Price = 146, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/15.png", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(280).Take(5))},

                new Vehicle {Manufacturer = "Kia", Model = "Rio", AverageGrade = 8.3,  Year = 2018, Doors = 4, Seats = 5, Price = 210, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/16.jpg", Type = "Small", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(285).Take(5))},
               
                new Vehicle {Manufacturer = "Mitsubishi", Model = "Mirage", AverageGrade = 8.2, Year = 2017, Doors = 4, Seats = 4, Price = 365, 
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/17.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(290).Take(5))},
               
                new Vehicle {Manufacturer = "Volkswagen", Model = "Golf SportWagen", AverageGrade = 7.9,  Year = 2018, Doors = 4, Seats = 5, Price = 180,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/24.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(295).Take(5))},

                new Vehicle {Manufacturer = "Volvo", Model = "V90", AverageGrade = 6.6, Year = 2015, Doors = 4, Seats = 5, Price = 270,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/25.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(300).Take(5))},

                new Vehicle {Manufacturer = "Mercedes", Model = "AMG E63 S Wagon", AverageGrade = 7.6, Year = 2016, Doors = 4, Seats = 5, Price = 260,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/46.jpg", Type = "Medium",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(305).Take(5))},

                new Vehicle {Manufacturer = "Mercedes", Model = "AMG S63/S65", AverageGrade = 9.3, Year = 2019, Doors = 4, Seats = 5, Price = 340,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/27.jpg", Type = "Luxury",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(310).Take(5))},
               
                new Vehicle {Manufacturer = "Volkswagen", Model = "Golf GTI", AverageGrade = 9.9, Year = 2020, Doors = 4, Seats = 5, Price = 198,
                    IsDeleted = false, IsOnDiscount = false,
                    Photo = "http://localhost:5000/23.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(315).Take(5))},

                new Vehicle {Manufacturer = "Hyundai", Model = "Ioniq", AverageGrade = 6.6, Year = 2014, Doors = 4, Seats = 5, Price = 175,
                    IsDeleted = false, IsOnDiscount = false, 
                    Photo = "http://localhost:5000/18.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(320).Take(5))},

                new Vehicle {Manufacturer = "Kia", Model = "Cadenza", AverageGrade = 9.6, Year = 2019, Doors = 4, Seats = 5, Price = 320,
                    IsDeleted = false, IsOnDiscount = false, 
                    Photo = "http://localhost:5000/39.jpg", Type = "Medium",  Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(325).Take(5))},


                new Vehicle {Manufacturer = "Volvo", Model = "V90 Cross Country", AverageGrade = 7.9,  Year = 2016, Doors = 4, Seats = 5, Price = 275,
                    IsDeleted = false, OldPrice = 260, IsOnDiscount = true,
                    Photo = "http://localhost:5000/26.jpg", Type = "Large", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(330).Take(5))},

                new Vehicle {Manufacturer = "Porsche", Model = "911 Turbo/Turbo S", AverageGrade = 8.6, Year = 2014, Doors = 2, Seats = 3, Price = 310,
                    IsDeleted = false, OldPrice = 380, IsOnDiscount = true,
                    Photo = "http://localhost:5000/27.jpg", Type = "Luxury", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(335).Take(5))},
                
                new Vehicle {Manufacturer = "Chevrolet", Model = "Corvette Z06", AverageGrade = 8.7, Year = 2016, Doors = 2, Seats = 2, Price = 313,
                    IsDeleted = false, OldPrice = 370, IsOnDiscount = true,
                    Photo = "http://localhost:5000/30.jpg", Type = "Medium", Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(340).Take(5))},

                new Vehicle {Manufacturer = "Audi", Model = "A6", AverageGrade = 9.4, Year = 2019, Doors = 4, Seats = 5, Price = 290,
                    IsDeleted = false, OldPrice = 360, IsOnDiscount = true,
                    Photo = "http://localhost:5000/4.jpg", Type = "Large",Ratings = new List<VehicleRating>(db.VehicleRatings.Skip(345).Take(5))},
            };

            return vehicles;
        }

        public static List<Flight> GetFlights(DataContext db)
        {
            Random random = new Random();

            List<Flight> flights = new List<Flight>();

            for (int i = 0; i < 600; i++)
            {
                var departureTime = DateTime.Now.AddDays(random.Next(1, 15)).AddHours(random.Next(1, 14)).AddMinutes(random.Next(1, 59)); 
                var arrivalTime = departureTime.AddHours(random.Next(1, 3)).AddMinutes(random.Next(1, 59));
                var ticketPrice = random.Next(100, 550);
                var mileage = random.Next(100, 1500);
                var avgGrade = random.NextDouble() * (10 - 6) + 6;
                var travelTime = (arrivalTime - departureTime).TotalMinutes;

                Flight flight = new Flight {
                    DepartureDestination = db.Destinations.Skip(random.Next(1, 9)).First(), 
                    ArrivalDestination = db.Destinations.Skip(random.Next(1, 9)).First(), 
                    DepartureTime = departureTime,
                    ArrivalTime = arrivalTime,
                    TravelTime = travelTime, 
                    AverageGrade = Math.Round(avgGrade,2),
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

            for(int i = 0; i < 350; i++)
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

