using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PDRProvBackEnd.Models;
using PDRProvBackEnd.DTOModels;
using Microsoft.Extensions.Configuration;
using PDRProvBackEnd.Repository;

namespace PDRProvBackEnd.Services
{
    public interface IUsersService
    {
        Task<UserModel> GetAsync(string Id);
        Task<List<PDRProvBackEnd.DTOModels.RoleModel>> GetRolesAsync(string Id);
        Task<UserModel> AuthenticateAsync(AuthModel loginRequest);
        Task<User> ChangePassword(AuthModel authModel);
        Task<User> RequestChange(string Email);
        Task<bool> CreateUser(User authModel);
    }
}
