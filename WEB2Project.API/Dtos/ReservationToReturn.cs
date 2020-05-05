using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WEB2Project.Models;

namespace WEB2Project.Dtos
{
    public class ReservationToReturn
    {
        public int Id { get; set; }
        public string UserAuthId { get; set; }
        public virtual VehicleToReturn Vehicle { get; set; }
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
