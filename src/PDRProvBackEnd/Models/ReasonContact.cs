using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Models
{
    public class ReasonContact
    {
        [Key]
        public int  Id { get; set; }

        [Display(Name = "Motivo")]
        [StringLength(256)]
        public string Reason { get; set; }

        [Display(Name = "Clasificacion Contacto")]
        public int TypeContact { get; set; }
    }
}
