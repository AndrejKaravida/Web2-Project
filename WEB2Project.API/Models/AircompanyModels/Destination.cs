using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Models
{
    public class Destination
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string MapString { get; set; }
        public ICollection<CompanyDestination> CompanyDestinations { get; set; }
    }
}
