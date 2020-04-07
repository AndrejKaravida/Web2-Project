using WEB2Project.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.API.Data
{
    public class DataContext : IdentityDbContext<User, Role, int, IdentityUserClaim<int>, 
    UserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) {}

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<CompanyRating> CompanyRatings { get; set; }
        public DbSet<VehicleRating> VehicleRatings { get; set; }
        public DbSet<Flight> Flights { get; set; }
        public DbSet<AirCompany> AirCompanies { get; set; }
        public DbSet<Vehicle> Vehicles { get; set; }
        public DbSet<VehicleOnDiscount> VehiclesOnDiscount { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<RentACarCompany> RentACarCompanies { get; set; }
        public DbSet<Destination> Destinations { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
         {

             base.OnModelCreating(builder);

             builder.Entity<UserRole>(userRole => { 
                 userRole.HasKey(ur => new {ur.UserId, ur.RoleId});

                 userRole.HasOne(ur => ur.Role)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(ur => ur.RoleId)
                 .IsRequired();

                 userRole.HasOne(ur => ur.User)
                 .WithMany(r => r.UserRoles)
                 .HasForeignKey(ur => ur.UserId)
                 .IsRequired();

             });
         }
    }
}