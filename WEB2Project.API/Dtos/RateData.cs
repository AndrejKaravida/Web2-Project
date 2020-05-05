using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Dtos
{
    public class RateData
    {
        public string UserId { get; set; }
        public int ReservationId { get; set; }
        public int CompanyId { get; set; }
        public int VehicleId { get; set; }
        public int CompanyRating { get; set; }
        public int VehicleRating { get; set; }
    }
}
