using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.DTOModels
{
    public class SupplierModel
    {
        [Display(Name = "Rut")]
        [StringLength(256)]
        public string Id { get; set; }
        [Display(Name = "Rut")]
        [StringLength(256)]
        public string Rut { get; set; }
        [Display(Name = "Nombre")]
        [StringLength(256)]
        public string NameSupplier { get; set; }
        [Display(Name = "Giro")]
        [StringLength(256)]
        public string Giro { get; set; }
        [Display(Name = "Nombre Contacto")]
        [StringLength(256)]
        public string NameContact { get; set; }
        [Display(Name = "Numero cel contacto")]
        [StringLength(256)]
        public string PhoneContact { get; set; }
        [Display(Name = "Email")]
        [StringLength(256)]
        public string EmailContact { get; set; }
    }
}
