using System.Collections.Generic;
using WEB2Project.Models.RentacarModels;

namespace WEB2Project.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double AverageGrade { get; set; }
        public virtual ICollection<VehicleRating> Ratings { get; set; }
        public string CurrentDestination { get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }
        public int Price { get; set; }
        public string Photo { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsOnDiscount { get; set; }
        public int OldPrice { get; set; }
        public string Type { get; set; }
        public virtual ICollection<ReservedDate> ReservedDates { get; set; }
    }
}
