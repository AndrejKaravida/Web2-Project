using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Dtos
{
    public class SearchParams
    {
        public string Location { get; set; }
        public string StartingDate { get; set; }
        public string ReturningDate { get; set; }
    }
}
