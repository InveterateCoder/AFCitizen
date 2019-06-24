using System.ComponentModel.DataAnnotations;

namespace AFCitizen.Models.Admin
{
    public class RoleModificationMod
    {
        [Required]
        public string RoleName { get; set; }
        public string RoleId { get; set; }
        public string[] IdsToAdd { get; set; }
        public string[] IdsToDelete { get; set; }
    }
}
