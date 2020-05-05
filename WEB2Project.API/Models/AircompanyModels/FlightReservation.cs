using System;

namespace WEB2Project.API.Models.AircompanyModels
{
    public class FlightReservation
    {
         public int Id { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime ArrivalDate { get; set; }
        public double Price { get; set; }
        public double TravelLength { get; set; }
        //public string Seats { get; set; }
    }
}