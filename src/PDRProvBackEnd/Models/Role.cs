using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace PDRProvBackEnd.Models
{
    public class Role
    {
        [Key]
        [Display(Name = "Nombre de rol")]
        [StringLength(20)]
        public string Name { get; set; }
        [JsonIgnore]
        public List<User> Users { get; set; }
    }
}
