using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Models
{
    public class UserSupplier:EntityGlobal
    {
        public User User { get; set; }

        public Supplier Supplier { get; set; }
    }
}
