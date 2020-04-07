using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Models.RentacarModels
{
    public class VehicleOnDiscount
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double AverageGrade { get; set; }
        public virtual ICollection<VehicleRating> Ratings { get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }
        public int OldPrice { get; set; }
        public int NewPrice { get; set; }
        public string Photo { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsReserved { get; set; }
        public string Type { get; set; }
    }
}
