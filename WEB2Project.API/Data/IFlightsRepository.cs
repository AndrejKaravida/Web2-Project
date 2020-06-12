using System.Collections.Generic;
using System.Threading.Tasks;
using WEB2Project.API.Models.AircompanyModels;
using WEB2Project.Dtos;
using WEB2Project.Helpers;
using WEB2Project.Models;
using WEB2Project.Models.AircompanyModels;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Data
{
    public interface IFlightsRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        AirCompany GetCompany(int id);
        Flight GetFlight(int id);
        AirCompany GetCompanyWithFlights(int id);
        List<AirCompany> GetAllCompanies();
        List<Destination> GetAllDestinations();
        Destination GetDestination(string city);
        FlightReservation GetReservation(int id);
        PagedList<Flight> GetFlightsForCompany(int companyId, FlightsParams flightsParams);
        List<Flight> GetDiscountTicket(int companyId);
        List<AvioIncomes> GetAvioIncomes(int companyId);
        List<FlightReservation> GetFlightReservations(int companyId);
        List<FlightReservation> GetFlightReservationsForUser(string authId);
        Task<List<Flight>> GetFlights(FlightDto flightDto);
        AirCompany GetCompanyForFlight(int id);
        List<Branch> GetAllBranches();
    }
}
