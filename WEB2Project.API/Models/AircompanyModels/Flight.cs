﻿using System;
using System.Collections.Generic;
using WEB2Project.Models.AircompanyModels;

namespace WEB2Project.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public virtual Destination DepartureDestination { get; set; }
        public virtual Destination ArrivalDestination { get; set; }
        public double TravelTime { get; set; }
        public double Mileage { get; set; }
        public double AverageGrade {get;set;}
        public bool Discount { get; set; }
        public double TicketPrice { get; set; }
        public double Luggage { get; set; }
        public virtual ICollection<FlightRating> Ratings { get; set; }
    }
}
