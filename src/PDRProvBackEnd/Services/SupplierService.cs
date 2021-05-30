using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Models;
using PDRProvBackEnd.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PDRProvBackEnd.Services
{
    public class SupplierService : ISupplierService
    {

        private IConfiguration _configuration;
        private readonly ILogger<SupplierService> _log;
        private IPDRGlobal<Models.Supplier> _PdrRepo;
        public SupplierService(IPDRGlobal<Supplier> pawaRepo, IConfiguration configuration, ILogger<SupplierService> logger)
        {
            this._configuration = configuration;
            this._log = logger;
            this._PdrRepo = pawaRepo;
        }

        #region Create Supplier
        public async Task<bool> CreateSupplier(SupplierModel supplierModel)
        {
            try
            {
                using (var db = _PdrRepo.PDRContext)
                {
                    db.Add(supplierModel);
                    db.SaveChanges();
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion
            
        #region Edit supplier
        public async Task<Supplier> EditSupplier(Supplier supplier)
        {
            Supplier suppierResponse = new Supplier();
            using (var db = _PdrRepo.PDRContext)
            {
                suppierResponse = db.Suppliers.FirstOrDefault(x => x.Id == supplier.Id);
                if (suppierResponse != null)
                {

                    suppierResponse.Rut = supplier.Rut;
                    suppierResponse.NameSupplier = supplier.NameSupplier;
                    suppierResponse.Giro = supplier.Giro;
                    suppierResponse.NameContact = supplier.NameContact;
                    suppierResponse.PhoneContact = supplier.PhoneContact;
                    suppierResponse.EmailContact = supplier.EmailContact;
                    suppierResponse.HasContract = supplier.HasContract;
                    suppierResponse.HasOutsourcingService = supplier.HasOutsourcingService;
                    suppierResponse.HasEthicalManagement = supplier.HasEthicalManagement;
                    suppierResponse.HasCertification = supplier.HasCertification;
                    db.SaveChanges();

                }
            }
            return suppierResponse;
        }
        #endregion

        #region Get supplier by Id
        public async Task<Supplier> GetSupplierAsync(string Id)
        {
            Supplier response = _PdrRepo.PDRContext.Suppliers.FirstOrDefault(x => x.Id == Guid.Parse(Id));
            return response;
        }
        #endregion

        #region List supplier
        public async Task<List<Supplier>> ListSupplier()
        {
            List<Supplier> us = _PdrRepo.PDRContext.Set<Supplier>().ToList();
            return us;
        }
        #endregion

    }
}
