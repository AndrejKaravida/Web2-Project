﻿using Microsoft.AspNetCore.Authorization;
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
        [AllowAnonymous]
        public IActionResult GetAvioCompany(int id)
        {
            var company =  _repo.GetCompany(id);

            return Ok(company);
        }

        [HttpGet("aircompanies")]
        [AllowAnonymous]
        public IActionResult GetAllCompanies()
        {
            var companies = _repo.GetAllCompanies();

            return Ok(companies);
        }

        [HttpGet("destinations")]
        [AllowAnonymous]
        public IActionResult GetAllDestinations()
        {
            var destinations = _repo.GetAllDestinations();

            return Ok(destinations);
        }

        [HttpGet("getFlights/{companyId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFlightsForCompany(int companyId, [FromQuery]FlightsParams flightsParams)
        {
            var flights = await _repo.GetFlightsForCompany(companyId, flightsParams);

            Response.AddPagination(flights.CurrentPage, flights.PageSize,
             flights.TotalCount, flights.TotalPages);

            return Ok(flights);
        }

        [HttpPost("addCompany")]
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
                return CreatedAtRoute("GetRentACarCompany", new { id = company.Id }, company);
            else
                throw new Exception("Saving vehicle failed on save!");
        }

    }
}