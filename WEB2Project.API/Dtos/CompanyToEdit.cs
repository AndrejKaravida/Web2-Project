using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Dtos
{
    public class CompanyToEdit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PromoDescription { get; set; }
        public double WeekRentalDiscount { get; set; }
        public double MonthRentalDiscount { get; set; }
        public virtual User Admin { get; set; }
    }
}
