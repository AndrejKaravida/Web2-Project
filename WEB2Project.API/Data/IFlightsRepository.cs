using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Models;
using WEB2Project.Models;

namespace WEB2Project.Data
{
    public interface IFlightsRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();

        AirCompany GetCompany(int id);
        List<AirCompany> GetAllCompanies();

        List<User> GetUsers();
        Task<User> GetUser(int id);
    }
}
