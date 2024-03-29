﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
using WEB2Project.Dtos;
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
                .Include(b => b.Branches)
                .Include(h => h.HeadOffice)
                .Include(a => a.Admin)
                .ToList();
        }

        public async Task<List<Reservation>> GetCarReservationsForUser(string authId)
        {
            var reservations = await _context.Reservations.Where(x => x.UserAuthId == authId).Include(v => v.Vehicle).ToListAsync();

            return reservations;
        }

        public async Task<RentACarCompany> GetCompany(int id)
        {
            var company = await _context.RentACarCompanies
                .Include(b => b.Branches)
                .Include(r => r.Ratings)
                .Include(h => h.HeadOffice)
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(x => x.Id == id);
            return company;
        }

        public async Task<RentACarCompany> GetCompanyWithVehicles(int id)
        {
            var company = await _context.RentACarCompanies
                .Include(b => b.Branches)
                .Include(r => r.Ratings)
                .Include(v => v.Vehicles)
                .Include(h => h.HeadOffice)
                .Include(a => a.Admin)
                .FirstOrDefaultAsync(x => x.Id == id);
            return company;
        }

        public List<Income> GetCompanyIncomes(int companyId)
        {
            return _context.RentACarCompanies.Include(i => i.Incomes).FirstOrDefault(x => x.Id == companyId).Incomes.ToList();
        }

        public List<Reservation> GetCompanyReservations(int companyId)
        {
            return _context.Reservations.Where(x => x.CompanyId == companyId).Include(v => v.Vehicle).ToList();
        }

        public List<Vehicle> GetDiscountedVehicles(int companyId)
        {
            return _context.RentACarCompanies
                .Include(v => v.Vehicles)
                .ThenInclude(r => r.Ratings)
                .Include(v => v.Vehicles)
                .ThenInclude(rd => rd.ReservedDates)
                .FirstOrDefault(x => x.Id == companyId)
                .Vehicles.Where(x => x.IsOnDiscount == true && x.IsDeleted == false)
                .ToList();
        }

        public Vehicle GetVehicle(int id)
        {
            var vehicle = _context.Vehicles
                .Include(r => r.Ratings)
                .Include(rd => rd.ReservedDates)
                
                .FirstOrDefault(x => x.Id == id);

            return vehicle;
        }

        public PagedList<Vehicle> GetVehiclesForCompany(int companyId, VehicleParams vehicleParams)
        {
            int minSeats = 2;
            int maxSeats = 6;
            int minDoors = 2;
            int maxDoors = 6;
            double averageRating = 0;
            string types = "";

            if (vehicleParams.tenrating)
                averageRating = 9.5;
            if (vehicleParams.ninerating)
                averageRating = 9;
            if (vehicleParams.eightrating)
                averageRating = 8;
            if (vehicleParams.sevenrating)
                averageRating = 7;

            if (vehicleParams.smalltype)
                types += "small,";
            if (vehicleParams.mediumtype)
                types += "medium,";
            if (vehicleParams.largetype)
                types += "large,";
            if (vehicleParams.luxurytype)
                types += "luxury";

            if (vehicleParams.twodoors)
            {
                maxDoors = 2;
            }
            if (vehicleParams.fourdoors)
            {
                maxDoors = 4;
                minDoors = 4;
                if (vehicleParams.twodoors)
                    minDoors = 2;
            }
            if (vehicleParams.fivedoors)
            {
                maxDoors = 6;
                minDoors = 5;
                if (vehicleParams.fourdoors)
                    minDoors = 4;
                if (vehicleParams.twodoors)
                    minDoors = 2;
            }

            if (vehicleParams.twoseats)
            {
                maxSeats = 2;
            }
            if (vehicleParams.fiveseats)
            {
                maxSeats = 5;
                minSeats = 3;
                if (vehicleParams.twoseats)
                    minSeats = 2;
            }
            if (vehicleParams.sixseats)
            {
                maxSeats = 6;
                minSeats = 5;
                if (vehicleParams.fiveseats)
                    minSeats = 3;
                if (vehicleParams.twoseats)
                    minSeats = 2;
            }

            var vehicles = _context.RentACarCompanies
             .Include(v => v.Vehicles)
             .ThenInclude(rd => rd.ReservedDates)
             .FirstOrDefaultAsync(x => x.Id == companyId)
             .Result.Vehicles
             .Where(p => p.Price >= vehicleParams.minPrice && p.Price <= vehicleParams.maxPrice
              && p.Doors >= minDoors && p.Doors <= maxDoors
              && p.Seats >= minSeats && p.Seats <= maxSeats
              && p.AverageGrade >= averageRating && p.IsDeleted == false
              && p.IsOnDiscount == false)
             .ToList();

            var typesArray = types.Split(',');
            vehicles = vehicles.Where(p => typesArray.Contains(p.Type.ToLower())).ToList();
         
            if (vehicleParams.pickupLocation != null)
            {
                if (vehicleParams.pickupLocation.Length > 0)
                {
                    vehicles = vehicles.Where(p => p.CurrentDestination == vehicleParams.pickupLocation).ToList();
                }
            }

            if(vehicleParams.startingDate != null && vehicleParams.returningDate != null)
            {
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
                }
            }

            return PagedList<Vehicle>.CreateAsync(vehicles, vehicleParams.PageNumber, vehicleParams.PageSize);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public Reservation GetReservation(int id)
        {
            return _context.Reservations.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<List<RentACarCompany>> GetCompaniesWithCriteria(SearchParams searchParams)
        {
            var companies = await _context.RentACarCompanies
                .Include(x => x.Branches)
                .Include(v => v.Vehicles)
                .ThenInclude(a => a.ReservedDates)
                .Where(x => x.Branches.Any(c => c.City.ToLower() == searchParams.Location.ToLower()))
                .ToListAsync();

            DateTime start = DateTime.Parse(searchParams.StartingDate);
            DateTime end = DateTime.Parse(searchParams.ReturningDate);

            List<ReservedDate> reservedDates = new List<ReservedDate>();

            for (var dt = start; dt <= end; dt = dt.AddDays(1))
            {
                ReservedDate date = new ReservedDate { Date = dt };
                reservedDates.Add(date);
            }

            foreach(var company in companies)
            {
                var totalVehicles = company.Vehicles.Count;
                int current = 0;

                foreach (var reservedDate in reservedDates)
                {
                    foreach (var vehicle in company.Vehicles.ToList())
                    {
                        foreach (var date in vehicle.ReservedDates)
                        {
                            if (date.Date == reservedDate.Date)
                            {
                                current++;
                                if(current == totalVehicles)
                                {
                                    companies.Remove(company);
                                }
                            }
                        }
                    }
                }
            }

            return companies;
        }

        public async Task<List<Branch>> GetBranches()
        {
            return await _context.Branches.ToListAsync();
        }

        public List<Vehicle> GetDiscountedVehiclesForUser(int companyId, DiscountedVehiclesParams vehiclesParams)
        {
            var vehicles = _context.RentACarCompanies
                        .Include(v => v.Vehicles)
                        .ThenInclude(r => r.Ratings)
                        .Include(v => v.Vehicles)
                        .ThenInclude(rd => rd.ReservedDates)
                        .FirstOrDefault(x => x.Id == companyId)
                        .Vehicles.Where(x => x.IsOnDiscount == true && x.IsDeleted == false &&
                        x.CurrentDestination.ToLower() == vehiclesParams.pickupLocation.ToLower())
                        .ToList();

            DateTime start = DateTime.Parse(vehiclesParams.startingDate);
            DateTime end = start.AddDays(vehiclesParams.numberOfDays);

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
                    foreach (var date in vehicle.ReservedDates)
                    {
                        if (date.Date == reservedDate.Date)
                        {
                            vehicles.Remove(vehicle);
                            break;
                        }
                    }
                }
            }

            return vehicles;
        }
    }
}
