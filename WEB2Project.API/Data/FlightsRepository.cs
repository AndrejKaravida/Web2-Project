using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
using WEB2Project.API.Models.AircompanyModels;
using WEB2Project.Helpers;
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

        public void EditAvioCompany(AirCompany companyToEdit)
        {
            throw new NotImplementedException();
        }

        public List<AirCompany> GetAllCompanies()
        {
            var companies = _context.AirCompanies.Include(h => h.HeadOffice).Include(a => a.Admin)
                .ToList();

            return companies;
        }

        public List<Destination> GetAllDestinations()
        {
            var destinations = _context.Destinations.ToList();

            return destinations;
        }

        public AirCompany GetCompany(int id)
        {
            var company = _context.AirCompanies
                .Include(h => h.HeadOffice)
                .Include(a => a.Admin)
                .FirstOrDefault(x => x.Id == id);

            return company;
        }

        public AirCompany GetCompanyWithFlights(int id)
        {
            return _context.AirCompanies.Include(f => f.Flights).Where(x => x.Id == id).FirstOrDefault();
        }

        public Destination GetDestination(string city)
        {
            return _context.Destinations.Where(d => d.City == city).FirstOrDefault();
        }

        public List<Flight> GetDiscountTicket(int companyId)
        {
            return _context.AirCompanies
                .Include(f => f.Flights)
                .ThenInclude(d => d.DepartureDestination)
                .Include(f => f.Flights)
                .ThenInclude(a => a.ArrivalDestination)
                .FirstOrDefault(x => x.Id == companyId)
                .Flights
                .Where(x => x.Discount == true)
                .ToList();
        }

        public Flight GetFlight(int id)
        {
            return _context.Flights.Where(x => x.Id == id).Include(r => r.Ratings).FirstOrDefault();
        }

        public List<FlightReservation> GetFlightReservations(int companyId)
        {
            return _context.FlightReservations.Where(x => x.Id == companyId).ToList();
        }

        public PagedList<Flight> GetFlightsForCompany(int companyId, FlightsParams flightsParams)
        {
            if(flightsParams.DepartureDate == null)
            {
                var flights = _context.AirCompanies
                .Include(f => f.Flights)
                .ThenInclude(a => a.ArrivalDestination)
                .Include(f => f.Flights)
                .ThenInclude(d => d.DepartureDestination)
                .FirstOrDefault(x => x.Id == companyId)
                .Flights.ToList();

                return PagedList<Flight>.CreateAsync(flights, flightsParams.PageNumber, flightsParams.PageSize);
            }

            DateTime start = DateTime.ParseExact(flightsParams.DepartureDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(-5);
            DateTime end = DateTime.ParseExact(flightsParams.DepartureDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(5);
   
            var flights1 = _context.AirCompanies
                .Include(f => f.Flights)
                .ThenInclude(a => a.ArrivalDestination)
                .Include(f => f.Flights)
                .ThenInclude(d => d.DepartureDestination)
                .FirstOrDefault(x => x.Id == companyId)
                .Flights
                .Where(x => x.TicketPrice >= flightsParams.minPrice && x.TicketPrice <= flightsParams.maxPrice
                && x.DepartureDestination.City == flightsParams.DepartureDestination && x.ArrivalDestination.City == flightsParams.ArrivalDestination
                && x.DepartureTime >= start && x.DepartureTime <= end)
                .ToList();

            if (flightsParams.ReturningDate != null)
            {
                DateTime returningDate = DateTime.ParseExact(flightsParams.ReturningDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(1);
                var flights2 = _context.AirCompanies
                    .Include(f => f.Flights)
                    .ThenInclude(a => a.ArrivalDestination)
                    .Include(f => f.Flights)
                    .ThenInclude(d => d.DepartureDestination)
                    .FirstOrDefault(x => x.Id == companyId)
                    .Flights
                    .Where(x => x.TicketPrice >= flightsParams.minPrice && x.TicketPrice <= flightsParams.maxPrice
                    && x.DepartureDestination.City == flightsParams.ArrivalDestination && x.ArrivalDestination.City == flightsParams.DepartureDestination)
                    .ToList();

                var allFlights = new List<Flight>(flights1.Count + flights2.Count);
                allFlights.AddRange(flights1);
                allFlights.AddRange(flights2);

                return PagedList<Flight>.CreateAsync(allFlights, flightsParams.PageNumber, flightsParams.PageSize);
            }

            return PagedList<Flight>.CreateAsync(flights1, flightsParams.PageNumber, flightsParams.PageSize); 
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
