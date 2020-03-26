﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
using WEB2Project.Helpers;
using WEB2Project.Models;

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

        public async Task<PagedList<RentACarCompany>> GetAllCompanies(VehicleParams vehicleParams)
        {
            var companies = _context.RentACarCompanies
                .Include(v => v.Vehicles)
                .Include(l => l.Locations)
                .AsQueryable();

            if(vehicleParams != null)
            {
                if(vehicleParams.minPrice > 0)
                {
                    foreach (RentACarCompany company in companies)
                    {
                       foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if(vehicle.Price < vehicleParams.minPrice)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (vehicleParams.maxPrice < 500)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Price > vehicleParams.maxPrice)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (vehicleParams.minSeats > 0)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Seats < vehicleParams.minSeats)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (vehicleParams.maxSeats < 6)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Seats > vehicleParams.maxSeats)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (vehicleParams.minDoors > 0)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Doors < vehicleParams.minDoors)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (vehicleParams.maxDoors < 6)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Doors > vehicleParams.maxDoors)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }
            }

            return await PagedList<RentACarCompany>.CreateAsync(companies, vehicleParams.PageNumber, vehicleParams.PageSize);
        }

        public async Task<RentACarCompany> GetCompany(int id)
        {
            var company = await _context.RentACarCompanies.Include(v => v.Vehicles).Include(l => l.Locations).FirstOrDefaultAsync(x => x.Id == id);

            return company;
        }

        public List<Vehicle> GetVehiclesForCompany(int companyId, VehicleParams vehicleParams)
        {
            var vehicles = _context.RentACarCompanies
             .Include(v => v.Vehicles)
             .FirstOrDefaultAsync(x => x.Id == companyId)
             .Result.Vehicles
             .Where(p => p.Price >= vehicleParams.minPrice && p.Price <= vehicleParams.maxPrice
              && p.Doors >= vehicleParams.minDoors && p.Doors <= vehicleParams.maxDoors
              && p.Seats >= vehicleParams.minSeats && p.Seats <= vehicleParams.maxSeats
              && p.AverageGrade >= vehicleParams.averageRating).ToList();
              
            return vehicles;
        }

        public List<Vehicle> GetVehiclesForCompanyWithoutParams(int companyId)
        {
            var vehicles = _context.RentACarCompanies
             .Include(v => v.Vehicles)
             .FirstOrDefaultAsync(x => x.Id == companyId)
             .Result.Vehicles.ToList();

            return vehicles;
        }


        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
