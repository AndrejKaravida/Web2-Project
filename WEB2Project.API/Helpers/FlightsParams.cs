using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Helpers
{
    public class FlightsParams
    {
        public int minPrice { get; set; } 
        public int maxPrice { get; set; } 
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public string DepartureDate { get; set; }
        public string ReturningDate { get; set; }
    }
}
