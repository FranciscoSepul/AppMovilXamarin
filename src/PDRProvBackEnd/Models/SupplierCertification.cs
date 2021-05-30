using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Models
{
    public class SupplierCertification: EntityGlobal
    {
        public Supplier Supplier { get; set; }
        public string CertificationName { get; set; }        

    }
}
