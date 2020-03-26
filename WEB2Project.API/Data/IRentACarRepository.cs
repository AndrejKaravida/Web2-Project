using System.Collections.Generic;
using System.Threading.Tasks;
using WEB2Project.Helpers;
using WEB2Project.Models;

namespace WEB2Project.Data
{
    public interface IRentACarRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<RentACarCompany> GetCompany(int id);
        List<Vehicle> GetVehiclesForCompany(int companyId, VehicleParams vehicleParams);
        Task<PagedList<RentACarCompany>> GetAllCompanies(VehicleParams vehicleParams);
        List<Vehicle> GetVehiclesForCompanyWithoutParams(int companyId);
    }
}
