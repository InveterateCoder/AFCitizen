using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace AFCitizen.Models.Admin
{
    public class RoleEditMod
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<IdentityUser> Members { get; set; }
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }
}
