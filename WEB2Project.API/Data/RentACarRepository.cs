using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
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

        public List<RentACarCompany> GetAllCompanies()
        {
            var companies = _context.RentACarCompanies.Include(v => v.Vehicles).Include(l => l.Locations).ToList();

            return companies;
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
