using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.API.Data;
using WEB2Project.API.Models.AircompanyModels;
using WEB2Project.Dtos;
using WEB2Project.Helpers;
using WEB2Project.Models;
using WEB2Project.Models.AircompanyModels;

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
            var companies = _context.AirCompanies.Include(h => h.HeadOffice).Include(a => a.Admin)
                .ToList();

            return companies;
        }

        public List<Destination> GetAllDestinations()
        {
            var destinations = _context.Destinations.ToList();

            return destinations;
        }

        public List<AvioIncomes> GetAvioIncomes(int companyId)
        {
            return _context.AirCompanies.Include(i => i.Incomes).FirstOrDefault(x => x.Id == companyId).Incomes.ToList();
        }

        public AirCompany GetCompany(int id)
        {
            var company = _context.AirCompanies
                .Include(h => h.HeadOffice)
                .Include(a => a.Admin)
                .Include(r => r.Ratings)
                .FirstOrDefault(x => x.Id == id);

            return company;
        }

        public AirCompany GetCompanyForFlight(int id)
        {
            var companies = _context.AirCompanies.Include(f => f.Flights);

            foreach (var comp in companies)
            {
                foreach (var f in comp.Flights)
                {
                    if(f.Id == id)
                    {
                        return comp;
                    }
                }
            }

            return null;

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

        public List<FlightReservation> GetFlightReservationsForUser(string authId)
        {
            return _context.FlightReservations
                .Where(x => x.UserAuthId == authId)
                .Include(f => f.Flight)
                .ThenInclude(d => d.DepartureDestination)
                .Include(f => f.Flight)
                .ThenInclude(a => a.ArrivalDestination)
                .ToList();
        }

        public async Task<List<Flight>> GetFlights(FlightDto flightDto)
        {
            DateTime start = DateTime.ParseExact(flightDto.DepartureDate, "M/d/yyyy", CultureInfo.InvariantCulture);
            DateTime end = DateTime.ParseExact(flightDto.ArrivalDate, "M/d/yyyy", CultureInfo.InvariantCulture);

            return await _context.Flights
                .Include(d => d.DepartureDestination)
                .Include(a => a.ArrivalDestination)
                .Where(x => x.DepartureTime.Date == start.Date && 
                       x.ArrivalTime.Date == end.Date && 
                       x.ArrivalDestination.City == flightDto.ArrivalDestination && 
                       x.DepartureDestination.City == flightDto.StartingDestination)
                .ToListAsync();
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

            DateTime start = DateTime.ParseExact(flightsParams.DepartureDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(-7);
            DateTime end = DateTime.ParseExact(flightsParams.DepartureDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(7);
   
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
                DateTime startingDate = DateTime.ParseExact(flightsParams.DepartureDate, "M/d/yyyy", CultureInfo.InvariantCulture);
                DateTime returningDateStart = DateTime.ParseExact(flightsParams.ReturningDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(-7);
                DateTime returningDateEnd = DateTime.ParseExact(flightsParams.ReturningDate, "M/d/yyyy", CultureInfo.InvariantCulture).AddDays(7);

                if(returningDateStart < startingDate)
                {
                    returningDateStart = startingDate;
                }

                var flights2 = _context.AirCompanies
                    .Include(f => f.Flights)
                    .ThenInclude(a => a.ArrivalDestination)
                    .Include(f => f.Flights)
                    .ThenInclude(d => d.DepartureDestination)
                    .FirstOrDefault(x => x.Id == companyId)
                    .Flights
                    .Where(x => x.TicketPrice >= flightsParams.minPrice && x.TicketPrice <= flightsParams.maxPrice
                    && x.DepartureDestination.City == flightsParams.ArrivalDestination && x.ArrivalDestination.City == flightsParams.DepartureDestination
                      && x.DepartureTime >= returningDateStart && x.DepartureTime <= returningDateEnd)
                    .ToList();

                var allFlights = new List<Flight>(flights1.Count + flights2.Count);
                allFlights.AddRange(flights1);
                allFlights.AddRange(flights2);

                return PagedList<Flight>.CreateAsync(allFlights, flightsParams.PageNumber, flightsParams.PageSize);
            }

            return PagedList<Flight>.CreateAsync(flights1, flightsParams.PageNumber, flightsParams.PageSize); 
        }

        public FlightReservation GetReservation(int id)
        {
            return _context.FlightReservations.Where(x => x.Id == id).FirstOrDefault();
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

    }
}
