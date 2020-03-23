using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WEB2Project.API.Models
{
   public class User : IdentityUser<int>
    {
        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}