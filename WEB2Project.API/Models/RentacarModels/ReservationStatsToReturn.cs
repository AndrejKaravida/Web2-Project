using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Models.RentacarModels
{
    public class ReservationStatsToReturn
    {
        public int ReservationsToday { get; set; }
        public int ReservationsThisWeek { get; set; }
        public int ReservationsThisMonth { get; set; }
    }
}
