using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WEB2Project.API.Models
{
   public class Role : IdentityRole<int>
    
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}