using System;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace PDRProvBackEnd.DTOModels
{
    public class RoleModel
    {
        [Display(Name = "Nombre de rol")]
        [StringLength(20)]
        public string Name { get; set; }
    }
}
