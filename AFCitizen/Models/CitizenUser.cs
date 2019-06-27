using Microsoft.AspNetCore.Identity;

namespace AFCitizen.Models
{
    public class CitizenUser : IdentityUser
    {
        public string Dispatcher { get; set; }
        public string Position { get; set; }
    }
}
