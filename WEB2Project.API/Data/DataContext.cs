
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;
using WEB2Project.API.Models.AircompanyModels;
using WEB2Project.Dtos;
using Microsoft.EntityFrameworkCore;

namespace WEB2Project.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<FlightReservation> FlightReservations { get; set; }
        public DbSet<CompanyRating> CompanyRatings { get; set; }
        public DbSet<VehicleRating> VehicleRatings { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<AirCompany> AirCompanies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<RentACarCompany> RentACarCompanies { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Branch> Branches { get; set; }


    }

}