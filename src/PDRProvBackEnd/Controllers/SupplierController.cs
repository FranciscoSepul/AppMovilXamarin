using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Models;
using PDRProvBackEnd.Repository;
using PDRProvBackEnd.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace PDRProvBackEnd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("PDR/api/[controller]")]
    public class SupplierController : ControllerBase
    {
        private readonly ILogger<SupplierController> _logger;
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService SupplierService, ILogger<SupplierController> logger)
        {
            this._logger = logger;
            this._supplierService = SupplierService;
        }

        #region get supplier id
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpPost("Select supplier")]
        public async Task<IActionResult> GetSupplierByIdAsync([FromBody] RequestId request)
        {
            _logger.LogDebug("Consulta supplier {Idsupplier}", request.Id);
            try
            {
                Supplier supplier = await this._supplierService.GetSupplierAsync(request.Id);
                if (supplier != null)
                {
                    _logger.LogDebug("Encuentra supplier {Idsupplier}", request.Id);
                }
                else
                {
                    _logger.LogDebug("No encuentra supplier {Idsupplier}", request.Id);
                }
                return Ok(supplier);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en consulta supplier {Idsupplier}", request.Id);
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region crear supplier
        [HttpPost("registrar supplier")]
        public async Task<IActionResult> CreateSupplier([FromBody] SupplierModel supplier)
        {
            _logger.LogDebug("Crea supplier");
            try
            {
                Boolean supplie = await this._supplierService.CreateSupplier(supplier);
                if (supplie.Equals(false))
                {
                    _logger.LogDebug("No se creo supplier ");
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear supplier ");
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region List all supplier
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpPost("Listar supplier")]
        public async Task<IActionResult> ListAllSupplier()
        {
            _logger.LogDebug("Lista supplier ");
            try
            {
                List<Supplier> suppliersList =  await this._supplierService.ListSupplier();
                if (suppliersList != null)
                {
                    _logger.LogDebug("Encuentra suppliers ");
                }
                else
                {
                    _logger.LogDebug("No encuentra suppliers");
                }
                return Ok(suppliersList);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar supplier ");
                return BadRequest();
            }
        }
        #endregion

        #region Editar supplier        
        [HttpPost("Editar supplier")]
        public async Task<IActionResult> EditarSupplier([FromBody] Supplier supplier)
        {
            _logger.LogDebug("Editar supplier {Idsupplier}", supplier.Id);
            try
            {
                var supplierResponse = await this._supplierService.EditSupplier(supplier);
                return Ok(supplierResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al editar supplier {Idsupplier}",supplier.Id);
                return BadRequest();
            }
        }
        #endregion
    }
}
