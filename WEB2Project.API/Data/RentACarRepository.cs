using Microsoft.EntityFrameworkCore;
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

        public async Task<PagedList<RentACarCompany>> GetAllCompanies(CarCompanyParams companyParams)
        {
            var companies = _context.RentACarCompanies
                .Include(v => v.Vehicles)
                .Include(l => l.Locations)
                .AsQueryable();

            if(companyParams != null)
            {
                if(companyParams.minPrice > 0)
                {
                    foreach (RentACarCompany company in companies)
                    {
                       foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if(vehicle.Price < companyParams.minPrice)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (companyParams.maxPrice < 500)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Price > companyParams.maxPrice)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (companyParams.minSeats > 0)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Seats < companyParams.minSeats)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (companyParams.maxSeats < 6)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Seats > companyParams.maxSeats)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (companyParams.minDoors > 0)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Doors < companyParams.minDoors)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }

                if (companyParams.maxDoors < 6)
                {
                    foreach (RentACarCompany company in companies)
                    {
                        foreach (Vehicle vehicle in company.Vehicles)
                        {
                            if (vehicle.Doors > companyParams.maxDoors)
                            {
                                company.Vehicles.Remove(vehicle);
                            }
                        }
                    }
                }
            }

            return await PagedList<RentACarCompany>.CreateAsync(companies, companyParams.PageNumber, companyParams.PageSize);
        }

        public async Task<RentACarCompany> GetCompany(int id)
        {
            var company = await _context.RentACarCompanies.Include(v => v.Vehicles).Include(l => l.Locations).FirstOrDefaultAsync(x => x.Id == id);

            return company;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
