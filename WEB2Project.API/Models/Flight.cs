using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Models
{
    public class Flight
    {
        public int Id { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int AirCompanyId { get; set; }
        public string DepartureCity { get; set; }
        public string ArrivalCity { get; set; }

    }
}
