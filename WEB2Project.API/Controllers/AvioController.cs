﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.Data;
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
            var company =  _repo.GetCompany(id);

       //     List<Flight> companyFlights = company.Flights.ToList();
        //    companyFlights.RemoveRange(5, companyFlights.Count - 5);
        //    company.Flights = companyFlights;

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

        [HttpGet("getFlights/{companyId}")]
        public IActionResult GetFlightsForCompany(int companyId, [FromQuery]FlightsParams flightsParams)
        {
            var flights = _repo.GetFlightsForCompany(companyId, flightsParams);

            return Ok(flights);
        }

        [HttpGet("getFlightsPaging/{companyId}")]
        public async Task<IActionResult> GetFlightsForCompanyPaging(int companyId, [FromQuery]FlightsParams flightsParams)
        {
            var flights = await _repo.GetFlightsForCompanyPaging(companyId, flightsParams);

            Response.AddPagination(flights.CurrentPage, flights.PageSize,
             flights.TotalCount, flights.TotalPages);

            return Ok(flights);
        }

    }
}