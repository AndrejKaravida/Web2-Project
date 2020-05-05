﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Dtos
{
    public class CompanyToReturn
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromoDescription { get; set; }
        public double AverageGrade { get; set; }
        public double WeekRentalDiscount { get; set; }
        public double MonthRentalDiscount { get; set; }
        public virtual Branch HeadOffice { get; set; }
        public virtual ICollection<Branch> Branches { get; set; }
        public string Photo { get; set; }
        public virtual User Admin { get; set; }
    }
}
