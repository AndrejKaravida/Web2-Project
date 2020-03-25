namespace WEB2Project.Helpers
{
    public class CarCompanyParams
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
        public int maxPrice { get; set; } = 500;
        public int minSeats { get; set; } = 0;
        public int maxSeats { get; set; } = 6;
        public int minDoors { get; set; } = 0;
        public int maxDoors { get; set; } = 6;
        public int minRating { get; set; } = 7;
        public string types { get; set; } = "small, medium, large, luxury";
    }
}
