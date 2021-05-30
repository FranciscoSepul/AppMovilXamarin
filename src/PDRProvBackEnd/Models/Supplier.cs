using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Models
{
    public class Supplier : EntityGlobal
    {
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
        public DateTime? ApprovalDate{get;set;}
        [Display(Name = "Email")]
        [StringLength(256)]
        public string EmailContact { get; set; }
        public Boolean HasContract { get; set; }
        public Boolean HasOutsourcingService { get; set; }
        public Boolean HasEthicalManagement { get; set; }
        public Boolean HasCertification { get; set; }

    }
}
