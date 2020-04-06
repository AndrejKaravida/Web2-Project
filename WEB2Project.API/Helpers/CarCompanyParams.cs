namespace WEB2Project.Helpers
{
    public class VehicleParams
    {
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
