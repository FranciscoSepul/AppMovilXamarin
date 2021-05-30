using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PDRProvBackEnd.DTOModels
{
    public class AuthModel
    {
        [Display(Name = "Username")]
        [Required]
        [StringLength(256)]
        public string Username { get; set; }

        [Display(Name = "Password")] 
        [Required]
        [StringLength(256)]
        public string Password { get; set; }
    }
}
