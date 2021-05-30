using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.DTOModels
{
    public class RequestId
    {
        [Required(ErrorMessage = "ID no puede ser nulo o vacio")]
        [Display(Name = "Numero Identificador")]
        public string Id { get; set; }
    }
}
