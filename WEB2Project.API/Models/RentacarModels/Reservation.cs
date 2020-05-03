using System;

namespace WEB2Project.Models.RentacarModels
{
    public class Reservation
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public virtual Vehicle Vehicle { get; set; }
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int NumberOfDays { get; set; }
        public string StartingLocation { get; set; }
        public string ReturningLocation { get; set; }
        public double DaysLeft { get; set; }
        public double TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
