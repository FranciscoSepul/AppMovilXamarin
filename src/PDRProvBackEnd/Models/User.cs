using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PDRProvBackEnd.Models
{
    public class User : EntityGlobal
    {
        [Display(Name ="Username")]
        [StringLength(256)]
        public string Username { get; set; }

        [Display(Name ="Email")]
        [StringLength(256)]
        public string Email { get; set; }        

        [JsonIgnore]
        public byte[] PasswordHash { get; set; }
        [JsonIgnore]
        public byte[] PasswordSalt { get; set; }   

        public List<Role> Roles { get; set; }
    }
}