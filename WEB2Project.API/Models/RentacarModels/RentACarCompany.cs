using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Models
{
    public class RentACarCompany
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string PromoDescription { get; set; }
        public double AverageGrade { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public double WeekRentalDiscount { get; set; }
        public double MonthRentalDiscount { get; set; }
        public List<Income> Incomes { get; set; }
        public List<Location> Locations { get; set; }

    }
}
