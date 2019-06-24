﻿using System.ComponentModel.DataAnnotations;

namespace AFCitizen.Models
{
    public class LoginMod
    {
        [Required]
        [UIHint("email")]
        public string Email { get; set; }
        [Required]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
