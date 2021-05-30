using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Services;
using Microsoft.Extensions.Configuration;
using PDRProvBackEnd.Repository;
using PDRProvBackEnd.Models;

namespace PDRProvBackEnd.Controllers
{
    [Authorize]
    [ApiController]
    [Route("PDR/api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUsersService _userService;

        public UserController(ILogger<UserController> logger, IUsersService userService,
            IConfiguration configuration)
        {
            _logger = logger;
            this._userService = userService;
        }

        #region get User by Id
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByIdAsync([FromBody] RequestId request)
        {
            _logger.LogDebug("Consulta usuario {IdUser}",request.Id);
            try
            {
                var user = await this._userService.GetAsync(request.Id);
                if (user != null)
                    _logger.LogDebug("Encuentra usuario {IdUser}", request.Id);
                else
                    _logger.LogDebug("Usuario no encontrado {IdUser}", request.Id);

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en consulta por usuario {UserId}", request.Id);
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region crear usuario
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpGet("{Id}")]
        public async Task<IActionResult> CreateUser([FromBody] User user)
        {
            _logger.LogDebug("Crear usuario");
            try
            {
                var userResponse = await this._userService.CreateUser(user);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en crear usuario ");
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region Get Roles By UserId 
        [Authorize(Roles = Entities.Roles.Administrador)]
        [HttpGet("{Id}/roles")]
        public async Task<IActionResult> GetRolesByUserIdAsync(string Id)
        {
            _logger.LogDebug("Consulta roles de usuario {IdUser}", Id);
            try
            {
                var roles = await this._userService.GetRolesAsync(Id);
                if (roles != null)
                    _logger.LogDebug("Encuentra roles {IdUser}", Id);
                else
                    _logger.LogDebug("Usuario no tiene roles {IdUser}", Id);

                return Ok(roles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en consulta roles por usuario {UserId}", Id);
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region Authenticate User
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthModel loginRequest)
        {
            _logger.LogDebug("Autenticación de usuario");
            try
            {
                if (loginRequest == null ||
                    string.IsNullOrWhiteSpace(loginRequest.Password) ||
                    (string.IsNullOrWhiteSpace(loginRequest.Username) && string.IsNullOrWhiteSpace(loginRequest.Username)) ||
                    !ModelState.IsValid
                  )
                {
                    _logger.LogInformation("Solicitud sin datos necesarios {loginRequest}", loginRequest);
                    return BadRequest(new ResponseModel()
                    {
                        Code = -1,
                        Message = "Error en solicitud."
                    });
                }

                var user = await _userService.AuthenticateAsync(loginRequest);

                if (user == null)
                {
                    _logger.LogInformation("Acceso denegado {userName}", loginRequest.Username);
                    return Unauthorized(new ResponseModel()
                    {
                        Code = 1,
                        Message = "Usuario no válido."
                    });
                }
                else
                    _logger.LogInformation("Acceso otorgado {userName}", loginRequest.Username);

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en autenticación usuario {userName}", loginRequest.Username);
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region change password 
        [HttpPost("changepassword")]
        public async Task<IActionResult> ChangePassword([FromBody] AuthModel authModel)
        {
            try
            {
                var user = _userService.ChangePassword(authModel);
                if (user.Id.Equals(null))
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(ResponseModel.ErrorInterno());
                }

            }
            catch (Exception )
            {
                return BadRequest(ResponseModel.ErrorInterno());
            }
        }
        #endregion

        #region request mail change password
        [AllowAnonymous]
        [HttpPost("RequestPassword")]
        public async Task<IActionResult> RequestNewPassword([FromBody] RequestPasswordModel requestPasswordModel) //aca al user service 
        {
            if (requestPasswordModel.Email != null)
            {
                var user = _userService.RequestChange(requestPasswordModel.Email);
                if (user.Id>0)
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(ResponseModel.ErrorInterno());
                }
            }
            else
            {
                return BadRequest("El correo electronico no puede ser nulo o vacio");
            }

        }
        #endregion

    }
}
