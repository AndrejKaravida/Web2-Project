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

        public List<RentACarCompany> GetAllCompaniesNoPaging()
        {
            return _context.RentACarCompanies
                .Include(r => r.Ratings)
                .Include(v => v.Vehicles)
                .Include(l => l.Locations)
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
                .Include(v => v.Vehicles)
                .Include(r => r.Ratings)
                .Include(l => l.Locations)
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

        public Vehicle GetVehicle(int id)
        {
            var vehicle = _context.Vehicles.Include(r => r.Ratings).FirstOrDefault(x => x.Id == id);

            return vehicle;
        }

        public List<Vehicle> GetVehiclesForCompany(int companyId, VehicleParams vehicleParams)
        {
            var types = vehicleParams.types.Split(',');

            var vehicles = _context.RentACarCompanies
             .Include(v => v.Vehicles)
             .Include(r => r.Ratings)
             .FirstOrDefaultAsync(x => x.Id == companyId)
             .Result.Vehicles
             .Where(p => p.Price >= vehicleParams.minPrice && p.Price <= vehicleParams.maxPrice
              && p.Doors >= vehicleParams.minDoors && p.Doors <= vehicleParams.maxDoors
              && p.Seats >= vehicleParams.minSeats && p.Seats <= vehicleParams.maxSeats
              && p.AverageGrade >= vehicleParams.averageRating && types.Contains(p.Type.ToLower())).ToList();           
 
            return vehicles;
        }

        public List<Vehicle> GetVehiclesForCompanyWithoutParams(int companyId)
        {
            var vehicles = _context.RentACarCompanies
             .Include(v => v.Vehicles)
             .Include(r => r.Ratings)
             .FirstOrDefaultAsync(x => x.Id == companyId)
             .Result.Vehicles.Where(x => x.IsDeleted == false).ToList();

            return vehicles;
        }


        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
