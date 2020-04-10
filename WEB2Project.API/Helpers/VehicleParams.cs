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
        public int minSeats { get; set; } = 0;
        public int maxSeats { get; set; } = 6;
        public int minDoors { get; set; } = 0;
        public string PickupLocation { get; set; } = "";
        public int maxDoors { get; set; } = 6;
        public double averageRating { get; set; } = 0;
        public string types { get; set; } = "";
    }
}
