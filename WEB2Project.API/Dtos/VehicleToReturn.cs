using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Dtos
{
    public class VehicleToReturn
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double AverageGrade { get; set; }
        public string CurrentDestination { get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }
        public int Price { get; set; }
        public string Photo { get; set; }
        public string Type { get; set; }
    }
}
