using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
using WEB2Project.Helpers;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Data
{

    public class RentACarRepository : IRentACarRepository
    {
        private readonly DataContext _context;

        public RentACarRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public List<RentACarCompany> GetAllCompanies()
        {
            return _context.RentACarCompanies
                .Include(d => d.Destinations)
                .ToList();
        }

        public List<Reservation> GetCarReservationsForUser(string userName)
        {
            var reservations = _context.Reservations.Where(x => x.UserName == userName).Include(v => v.Vehicle).ToList();

            return reservations;
        }

        public async Task<RentACarCompany> GetCompany(int id)
        {
            var company = await _context.RentACarCompanies
                .Include(d => d.Destinations)
                .FirstOrDefaultAsync(x => x.Id == id);
            return company;
        }

        public List<Income> GetCompanyIncomes(int companyId)
        {
            return _context.RentACarCompanies.Include(i => i.Incomes).FirstOrDefault(x => x.Id == companyId).Incomes.ToList();
        }

        public List<Reservation> GetCompanyReservations(int companyId)
        {
            return _context.Reservations.Where(x => x.CompanyId == companyId).ToList();
        }

        public List<Vehicle> GetDiscountedVehicles(int companyId)
        {
            return _context.RentACarCompanies
                .Include(v => v.Vehicles)
                .ThenInclude(r => r.Ratings)
                .Include(v => v.Vehicles)
                .ThenInclude(rd => rd.ReservedDates)
                .FirstOrDefault(x => x.Id == companyId)
                .Vehicles.Where(x => x.IsOnDiscount == true)
                .ToList();
        }

        public Vehicle GetVehicle(int id)
        {
            var vehicle = _context.Vehicles.Include(r => r.Ratings).Include(rd => rd.ReservedDates).FirstOrDefault(x => x.Id == id);

            return vehicle;
        }

        public async Task<PagedList<Vehicle>> GetVehiclesForCompany(int companyId, VehicleParams vehicleParams)
        {
            var types = vehicleParams.types.Split(',');

            var vehicles = _context.RentACarCompanies
             .Include(v => v.Vehicles)
             .ThenInclude(rd => rd.ReservedDates)
             .FirstOrDefaultAsync(x => x.Id == companyId)
             .Result.Vehicles
             .Where(p => p.Price >= vehicleParams.minPrice && p.Price <= vehicleParams.maxPrice
              && p.Doors >= vehicleParams.minDoors && p.Doors <= vehicleParams.maxDoors
              && p.Seats >= vehicleParams.minSeats && p.Seats <= vehicleParams.maxSeats
              && p.AverageGrade >= vehicleParams.averageRating && p.IsDeleted == false
              && p.IsOnDiscount == false)
             .ToList();

            if(vehicleParams.types.Length > 0)
            {
                vehicles = vehicles.Where(p => types.Contains(p.Type.ToLower())).ToList();
            }
            if(vehicleParams.pickupLocation.Length > 0)
            {
                vehicles = vehicles.Where(p => p.CurrentDestination == vehicleParams.pickupLocation).ToList();
            }

            if(vehicleParams.startingDate.Length > 0 && vehicleParams.returningDate.Length > 0)
            {
               DateTime start = DateTime.Parse(vehicleParams.startingDate);
               DateTime end = DateTime.Parse(vehicleParams.returningDate);

               List<ReservedDate> reservedDates = new List<ReservedDate>();

               for (var dt = start; dt <= end; dt = dt.AddDays(1))
               {
                   ReservedDate date = new ReservedDate { Date = dt };
                   reservedDates.Add(date);
               }

               foreach (var reservedDate in reservedDates)
               {
                   foreach (var vehicle in vehicles.ToList())
                   {
                       foreach(var date in vehicle.ReservedDates)
                       {
                            if(date.Date == reservedDate.Date)
                            {
                                vehicles.Remove(vehicle);
                                break;
                            }
                       }
                   }    
               }
             //   vehicles = vehicles.Where(v => v.ReservedDates.Any(x => reservedDates.Contains(x))).ToList();
            //   vehicles = vehicles.Where(v => v.ReservedDates.Intersect(reservedDates).Any()).ToList();
            }

            return PagedList<Vehicle>.CreateAsync(vehicles, vehicleParams.PageNumber, vehicleParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
