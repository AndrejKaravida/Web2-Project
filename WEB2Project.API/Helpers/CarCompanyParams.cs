namespace WEB2Project.Helpers
{
    public class VehicleParams
    {
        private const int MaxPageSize = 30;
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }
        public int minPrice { get; set; } = 0;
        public int maxPrice { get; set; } = 400;
        public int minSeats { get; set; } = 0;
        public int maxSeats { get; set; } = 0;
        public int minDoors { get; set; } = 0;
        public int maxDoors { get; set; } = 0;
        public double averageRating { get; set; } = 0;
        public string types { get; set; } = "Small,Medium,Large,Luxury";
    }
}
