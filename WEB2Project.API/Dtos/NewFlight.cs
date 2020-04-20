using System;
using System.Collections.Generic;
using System.Text;
using WEB2Project.Models;

namespace WEB2Project.Dtos
{
    public class NewFlight
    {
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public DateTime DepartureTime { get; set; }
        public double AverageGrade { get; set; }
        public bool Discount { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int TravelDuration { get; set; }
        public double TravelLength { get; set; }   
        public double Price { get; set; }
    }
}
