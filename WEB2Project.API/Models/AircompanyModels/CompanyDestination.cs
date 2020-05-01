namespace WEB2Project.Models
{
    public class CompanyDestination
    {
        public int CompanyDestinationId { get; set; }
        public AirCompany Company { get; set; }
        public Destination Destination { get; set; }
    }
}