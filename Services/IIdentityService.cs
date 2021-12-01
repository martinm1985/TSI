using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Crud.Models;

namespace Crud.Services
{
    public interface IIdentityService
    {
        Task<object> RegisterUser(string username, string email, string password, string name, string surname);
        Task<object> Login(string username, string email);
        Task<object> Refresh(string username, string email);
        Task<UserData> GetUserInfo(ClaimsPrincipal email);
        Task<UserData> GetUserInfoById(string id);
        Task<UserData?> MakeAdmin(string userId);
        Task<UserData?> RemoveAdmin(string userId);
        Task ForgotPassword(string email);

    }
}
