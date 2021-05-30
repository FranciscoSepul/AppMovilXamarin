using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDRProvBackEnd.DTOModels;
using PDRProvBackEnd.Models;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using PDRProvBackEnd.Repository;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PDRProvBackEnd.Contexts;
using System.Net.Mail;

namespace PDRProvBackEnd.Services
{
    public class UsersService : IUsersService
    {
        private IConfiguration _configuration;
        private readonly ILogger<UsersService> _log;
        private IPDRGlobal<Models.User> _PdrRepo;
        public UsersService(IPDRGlobal<User> PdrRepo, IConfiguration configuration, ILogger<UsersService> logger)
        {
            this._configuration = configuration;
            this._log = logger;
            this._PdrRepo = PdrRepo;
        }

        public async Task<UserModel> GetAsync(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ArgumentNullException(nameof(Id));

            _log.LogDebug("Comienza búsqueda usuario {UserId}", Id);

            DTOModels.UserModel user = null;
            var u = await _PdrRepo.PDRContext.Users.AsNoTracking().SingleOrDefaultAsync(x => x.Id == Guid.Parse(Id));

            if (u == null)
            {
                _log.LogDebug("Usuario no encontrado por identificador {UserId}", Id);
                return null;
            }

            user = new DTOModels.UserModel()
            {
                Username = u.Username,
                Email = u.Email,
                Id = u.Id.ToString()
            };

            _log.LogDebug("Se obtiene usuario se crea DTO {Username}", u.Username);

            return user;
        }

        public async Task<List<RoleModel>> GetRolesAsync(string Id)
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ArgumentNullException("Id no especificado.");

            _log.LogDebug("Comienza búsqueda de roles de {UserId}", Id);

            var u = await _PdrRepo.PDRContext.Users.AsNoTracking()
                .Include(r => r.Roles)
                .SingleOrDefaultAsync(x => x.Id == Guid.Parse(Id));

            if (u == null || u.Roles == null)
            {
                _log.LogDebug("Sin roles. No tuvo respuesta por usuario {UserId}", Id);
                return null;
            }
            else
            {
                var listRoles = new List<DTOModels.RoleModel>();
                u.Roles.ForEach(i => { listRoles.Add(new DTOModels.RoleModel() { Name = i.Name }); });

                _log.LogDebug("Roles encontrados para {UserId}. Total roles {countRoles}", Id, (u.Roles == null) ? 0 : u.Roles.Count);

                return listRoles;
            }
        }

        public async Task<UserModel> AuthenticateAsync(DTOModels.AuthModel loginRequest)
        {
            try
            {

                if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password)) return null;

                _log.LogDebug("Comienza autenticación para usuario {userName}", loginRequest.Username);

                var user = await _PdrRepo.PDRContext.Set<Models.User>().AsNoTracking()
                    .Include(r => r.Roles)
                    .SingleOrDefaultAsync(x => x.Username == loginRequest.Username || x.Email == loginRequest.Username);

                if (user == null)
                {
                    _log.LogDebug("Usuario no encontrado {userName}", loginRequest.Username);
                    return null;
                }

                if (!VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
                {
                    _log.LogDebug("Contraseña inválida {userName}", loginRequest.Username);
                    return null;
                }

                _log.LogDebug("Se crea token para {userName}", loginRequest.Username);

                DateTime expires =
                    DateTime.UtcNow.AddSeconds(_configuration.GetValue<TimeSpan>("Security:TokenExpires").TotalSeconds);

                _log.LogDebug("Expira {expires} / {totalSeconds} segundos",
                    expires.ToString("yyyy-MM-dd HH:mm:ss"),
                    _configuration.GetValue<TimeSpan>("Security:TokenExpires").TotalSeconds);

                var token = this.GenerateJwtToken(user, expires);
                UserModel userModel = new UserModel();
                userModel.Email = user.Email;
                userModel.Username = user.Username;
                userModel.Id = user.Id.ToString();
                userModel.Password = null;
                userModel.Token = token;
                userModel.TokenExpires = expires;

                _log.LogDebug("Token {token}", token);

                return userModel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        #region Cambiar password
        public async Task<User> ChangePassword(AuthModel authModel)
        {
            var user = _PdrRepo.PDRContext.Users.FirstOrDefault(x => x.Username == authModel.Username);
            if (user != null)
            {
                using var db = _PdrRepo.PDRContext;
                byte[] passwordHash, passwordSalt = null;
                CreatePasswordHash(authModel.Password, out passwordHash, out passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
                db.SaveChanges();

            }
            return user;
        }
        #endregion

        #region Enviar password autogenerada con Smtp
        public async Task<User> RequestChange(string Email)
        {
            var user = _PdrRepo.PDRContext.Users.FirstOrDefault(x => x.Email == Email);
            if (user != null)
            {
                string pass = GenerarPassword(8);
                using (var db = _PdrRepo.PDRContext)
                {                   
                    byte[] passwordHash, passwordSalt = null;
                    CreatePasswordHash(pass, out passwordHash, out passwordSalt);
                    user.PasswordHash = passwordHash;
                    user.PasswordSalt = passwordSalt;
                    db.SaveChanges();
                }
                string Subject = "Correo Smtp PDR";
                string Body = "Contrasena: "+pass;
                Smtp(Email, Subject, Body);
            }
            return user;
        }
        #endregion

        #region crear user
        public async Task<bool> CreateUser(User authModel)
        {
            try
            {
                using var db = _PdrRepo.PDRContext;
                db.Add(authModel);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
        #endregion
        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            if (storedHash.Length != 64) throw new ArgumentException("Hash no válido");
            if (storedSalt.Length != 128) throw new ArgumentException("Salt no válido");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }

            }
            return true;
        }

        private string GenerateJwtToken(Models.User user, DateTime expires)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("Security:JWTSecret"));

            var claims = new ClaimsIdentity();
            //claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.AddClaim(new Claim(ClaimTypes.Name, user.Username.ToString()));
            //claims.AddClaim(new Claim(ClaimTypes.Email, user.Email.ToString()));
            //claims.AddClaim(new Claim(ClaimTypes.Expiration, expires.ToString("yyyy-MM-dd HH:mm:ss")));
            if (user.Roles != null)
                foreach (Role role in user.Roles)
                    claims.AddClaim(new Claim(ClaimTypes.Role, role.Name));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        #region Gerenar pass aleatoria 
        public static string GenerarPassword(int longitud)
        {
            string contraseña = string.Empty;
            string[] letras = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "ñ", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z",
                                "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"};
            Random EleccionAleatoria = new Random();

            for (int i = 0; i < longitud; i++)
            {
                int LetraAleatoria = EleccionAleatoria.Next(0, 100);
                int NumeroAleatorio = EleccionAleatoria.Next(0, 9);

                if (LetraAleatoria < letras.Length)
                {
                    contraseña += letras[LetraAleatoria];
                }
                else
                {
                    contraseña += NumeroAleatorio.ToString();
                }
            }
            return contraseña;
        }
        #endregion

        #region Smtp
        public void Smtp(string to, string subject, string body)
        {
            try
            {
                string Email =_configuration.GetValue<string>("Smtp:Email");
                string Pass = _configuration.GetValue<string>("Smtp:Password");
                MailMessage mail = new MailMessage();
                mail.To.Add(new MailAddress(to));
                mail.From = new MailAddress(Email);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.Priority = MailPriority.Normal;
                mail.SubjectEncoding = Encoding.GetEncoding("UTF-8");

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(Email,Pass);


                smtp.Send(mail);
            }
            catch (Exception )
            {
                throw ;
            }

        }

        #endregion

    }
}
