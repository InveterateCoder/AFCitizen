using System.ComponentModel.DataAnnotations;

namespace AFCitizen.Models.Admin
{
    public class CreateMod
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Email { get; set; }
        public string Dispatcher { get; set; }
        public string Position { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
