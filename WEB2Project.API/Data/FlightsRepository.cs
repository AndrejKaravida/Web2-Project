using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
using WEB2Project.API.Models;
using WEB2Project.Models;

namespace WEB2Project.Data
{
    public class FlightsRepository : IFlightsRepository
    {

        private readonly DataContext _context;
        public FlightsRepository(DataContext context)
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

        public List<AirCompany> GetAllCompanies()
        {
            var companies =  _context.AirCompanies.ToList();

            return companies;
        }

        public AirCompany GetCompany(int id)
        {
            var company = _context.AirCompanies.FirstOrDefault(x => x.Id == id);

            return company;
        }

        public async Task<User> GetUser(int id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

            return user;
        }

        public async Task<List<User>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();

            return users;
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
