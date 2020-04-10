using System.Collections.Generic;
using System.Threading.Tasks;
using WEB2Project.Helpers;
using WEB2Project.Models;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Data
{
    public interface IRentACarRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<RentACarCompany> GetCompany(int id);
        Vehicle GetVehicle(int id);
        Task<PagedList<Vehicle>> GetVehiclesForCompany(int companyId, VehicleParams vehicleParams);
        List<RentACarCompany> GetAllCompanies();
        List<Vehicle> GetDiscountedVehicles(int companyId);
        List<Reservation> GetCarReservationsForUser(string userName);
        List<Income> GetCompanyIncomes(int companyId);
        List<Reservation> GetCompanyReservations(int companyId);
    }
}
