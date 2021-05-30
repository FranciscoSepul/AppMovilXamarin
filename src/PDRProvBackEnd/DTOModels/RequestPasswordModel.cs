using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.DTOModels
{
    public class RequestPasswordModel
    {
        [Required(ErrorMessage = "Email no puede ser nulo o vacio")]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }
}
