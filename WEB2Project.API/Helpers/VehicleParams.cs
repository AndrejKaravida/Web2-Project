using System;

namespace WEB2Project.Helpers
{
    public class VehicleParams
    {
        private const int MaxPageSize = 5;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 5;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public int minPrice { get; set; } = 0;
        public int maxPrice { get; set; } = 400;
        public bool twoseats { get; set; } = true;
        public bool fiveseats { get; set; } = true;
        public bool sixseats { get; set; } = true;
        public bool twodoors { get; set; } = true;
        public bool fourdoors { get; set; } = true;
        public bool fivedoors { get; set; } = true;
        public bool smalltype { get; set; } = true;
        public bool mediumtype { get; set; } = true;
        public bool largetype { get; set; } = true;
        public bool luxurytype { get; set; } = true;
        public bool sevenrating { get; set; } = true;
        public bool eightrating { get; set; } = true;
        public bool ninerating { get; set; } = true;
        public bool tenrating { get; set; } = true;
        public string pickupLocation { get; set; } = "";
        public string startingDate { get; set; } = "";
        public string returningDate { get; set; } = "";

    }
}
