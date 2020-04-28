﻿using System.Collections.Generic;

namespace WEB2Project.Models
{
    public class AirCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromoDescription { get; set; }
        public double AverageGrade { get; set; }
        public string Photo { get; set; }
        public virtual Destination HeadOffice { get; set; }
        public virtual ICollection<Flight> Flights { get; set; }
        public string Admin { get; set; }
    }
}
