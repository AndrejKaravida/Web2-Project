using System;
using System.Collections.Generic;
using System.Text;

namespace WEB2Project.Dtos
{
    public class FlightToEdit
    {
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public double TravelTime { get; set; }
        public double Mileage { get; set; }
        public double AverageGrade { get; set; }
        public bool Discount { get; set; }
        public double TicketPrice { get; set; }
        public double Luggage { get; set; }
        public int companyId { get; set; }
    }
}
