using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2Project.Dtos
{
    public class UserToCreate
    {
        public string given_name { get; set; }
        public string family_name { get; set; }
        public string name { get; set; }
        public string connection { get; set; }
        public string email { get; set; }
        public bool email_verified { get; set; }
        public string password { get; set; }
    }
}
