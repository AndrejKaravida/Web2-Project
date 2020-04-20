using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WEB2Project.Data;
using WEB2Project.Dtos;
using WEB2Project.Helpers;
using WEB2Project.Models;

namespace WEB2Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvioController : ControllerBase
    {
        private readonly IFlightsRepository _repo;
        public AvioController(IFlightsRepository repo)
        {
            _repo = repo;

        }
        [HttpGet("getCompany/{id}", Name = "GetAvioCompany")]
        public IActionResult GetAvioCompany(int id)
        {
            var company = _repo.GetCompany(id);

            return Ok(company);
        }

        [HttpGet("aircompanies")]
        public IActionResult GetAllCompanies()
        {
            var companies = _repo.GetAllCompanies();

            return Ok(companies);
        }

        [HttpGet("destinations")]
        public IActionResult GetAllDestinations()
        {
            var destinations = _repo.GetAllDestinations();

            return Ok(destinations);
        }

        [HttpPost("addFlight/{companyId}")]
        public async Task<IActionResult> MakeNewFlight(int companyId, NewFlight newFlight)
        {
            var departureDest = _repo.GetDestination(newFlight.DepartureDestination);
            var arrivalDest = _repo.GetDestination(newFlight.ArrivalDestination);

            //sad kad prosledjujes sa fronta moras proslediti i ova dva nova polaj i onda stavi brejk ovde vidi jel se sve dobro popuni i to je to poz
            //mogu ba postamn da proverim mozs
            //jebalo te sarajevo sarajevo ne postoji ub azi


            Flight flight = new Flight()
            {
                DepartureDestination = departureDest,
                ArrivalDestination = arrivalDest,
                AverageGrade = 6.6,
                DepartureTime = newFlight.DepartureTime,
                ArrivalTime = newFlight.ArrivalTime,
                Discount = false,
                Mileage = newFlight.TravelLength,
                TravelTime = newFlight.TravelDuration,
                TicketPrice = newFlight.Price
            };

            _repo.Add(flight);

            var companyFromRepo = _repo.GetCompanyWithFlights(companyId);
            companyFromRepo.Flights.Add(flight);

            if (await _repo.SaveAll())
                return NoContent();
            else
                throw new Exception("Saving flight failed on save!");
        }

        [HttpGet("getFlights/{companyId}")]
        public async Task<IActionResult> GetFlightsForCompany(int companyId, [FromQuery]FlightsParams flightsParams)
        {
            var flights = await _repo.GetFlightsForCompany(companyId, flightsParams);

            Response.AddPagination(flights.CurrentPage, flights.PageSize,
             flights.TotalCount, flights.TotalPages);

            return Ok(flights);
        }

        [HttpPost("addCompany")]
        [Authorize]
        public async Task<IActionResult> MakeNewCompany(CompanyToMake companyToMake)
        {

            Destination destination = new Destination();
            destination.City = companyToMake.City;
            destination.Country = companyToMake.Country;
            destination.MapString = companyToMake.MapString;

            _repo.Add(destination);
            await _repo.SaveAll();

            AirCompany company = new AirCompany()
            {
                Name = companyToMake.Name,
                PromoDescription = "Temporary promo description",
                AverageGrade = 0,
                HeadOffice = destination,
            };

            _repo.Add(company);


            if (await _repo.SaveAll())
                return NoContent();
            else
                throw new Exception("Saving vehicle failed on save!");
        }

        

        [HttpGet("getDiscountedFlights/{companyId}")]
        public List<Flight> GetDiscountedFlights(int companyId)
        {
            return _repo.GetDiscountTicket(companyId);    
        }

    }
}