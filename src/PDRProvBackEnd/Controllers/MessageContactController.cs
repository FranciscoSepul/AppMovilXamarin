using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Models;
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
    public class MessageContactController : ControllerBase
    {
        private readonly ILogger<MessageContactController> _logger;
        private readonly IMessageContact _messageContact;

        public MessageContactController(ILogger<MessageContactController> logger, IMessageContact messageContact)
        {
            _logger = logger;
            this._messageContact = messageContact;
        }

        #region get MessageContact
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpPost("Select Message")]
        public async Task<IActionResult> GetMessageContacByIdAsync([FromBody] RequestId request)
        {
            _logger.LogDebug("Consulta MessageContact {IdMessage}", request.Id);
            try
            {
                MessageContact messageContact = await this._messageContact.MessageContact(request.Id);
                if (messageContact !=null)
                {
                    _logger.LogDebug("MessageContact encontrado {IdMessage}", request.Id);
                }
                else
                {
                    _logger.LogDebug("MessageContact no encontrado {IdMessage}", request.Id);
                }
                return Ok(messageContact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en consultar MessageContact por Id {IdMessage}",request.Id);
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region crear MessageContac
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpPost("Create Message")]
        public async Task<IActionResult> CreateMessageContact(MessageContactModel messageContact)
        {
            _logger.LogDebug("crear MessageContact {IdMessage}", messageContact.Id);
            try
            {
                MessageContactModel messageContacts = await this._messageContact.CreateMessageContact(messageContact);
                if (messageContact != null)
                {
                    _logger.LogDebug("se creo MessageContact {IdMessage}", messageContact.Id);
                }
                else
                {
                    _logger.LogDebug("No fue posible crear  MessageContact {IdMessage}", messageContact.Id);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en Crear MessageContact {IdMessage}", messageContact.Id);
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region List all MessageContact
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpPost("Select all Message")]
        public async Task<IActionResult> AllMessageContact()
        {
            _logger.LogDebug("Consulta List MessageContact ");
            try
            {
               List<MessageContact> messageContact = await this._messageContact.ListMessageContact();
                if (messageContact!=null)
                {
                    _logger.LogDebug("Se encuentran MessageContact para listar");
                }
                else
                {
                    _logger.LogDebug("No se encuentran MessageContact para listar");
                }
                return Ok(messageContact);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al consultar MessageContact ");
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region Editar MessageContact
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpPost("Edit Message")]
        public async Task<IActionResult> EditMessageContact(MessageContactModel messageContact)
        {
            _logger.LogDebug("Editar MessageContact {IdMessage}",messageContact.Id);
            try
            {
                var messageContac = await this._messageContact.EditMessageContact(messageContact);
                if (messageContac != null)
                {
                    _logger.LogDebug("Se edito correctamente MessageContact {IdMessage}", messageContact.Id);
                }
                else
                {
                    _logger.LogDebug("No se realizo la edicion de MessageContact {IdMessage}", messageContact.Id);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en editar MessageContact {IdMessage}", messageContact.Id);
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion
    }
}
