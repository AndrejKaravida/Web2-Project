using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Helpers
{
    public class FlightsParams
    {
        private const int MaxPageSize = 10;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public int minPrice { get; set; } 
        public int maxPrice { get; set; } 
        public string DepartureDestination { get; set; }
        public string ArrivalDestination { get; set; }
        public string DepartureDate { get; set; }
        public string ReturningDate { get; set; }
    }
}
