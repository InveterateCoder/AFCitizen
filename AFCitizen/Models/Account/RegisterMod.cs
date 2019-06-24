using System.ComponentModel.DataAnnotations;

namespace AFCitizen.Models.Account
{
    public class RegisterMod
    {
        [Required]
        public string User { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}