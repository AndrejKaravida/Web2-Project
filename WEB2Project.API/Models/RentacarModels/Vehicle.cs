using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public double AverageGrade { get; set; }
        public int Doors { get; set; }
        public int Seats { get; set; }
        public int Price { get; set; }
    }
}
