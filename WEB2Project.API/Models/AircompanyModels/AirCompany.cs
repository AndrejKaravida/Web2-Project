﻿using System.Collections.Generic;
using WEB2Project.Dtos;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Models
{
    public class AirCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromoDescription { get; set; }
        public double AverageGrade { get; set; }
        public string Photo { get; set; }
        public virtual Branch HeadOffice { get; set; }
        public virtual ICollection<Flight> Flights { get; set; }
        public virtual User Admin { get; set; }
        public virtual ICollection<CompanyRating> Ratings { get; set; }

    }
}
