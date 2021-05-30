using PDRProvBackEnd.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Services
{
    public interface ISupplierService
    {
        Task<Supplier> GetSupplierAsync(string Id);
        Task<bool> CreateSupplier(DTOModels.SupplierModel supplierModel);
        Task<List<Supplier>>ListSupplier();
        Task<Supplier> EditSupplier(Models.Supplier supplier);
    }
}
