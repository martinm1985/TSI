using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.DTOs;
using System.Security.Claims;

namespace Crud.Services
{
    public interface IIdentityService
    {
        Task<object> RegisterUser(string username, string email, string password, string name, string surname);
        Task<object> Login(string username, string email);
        Task<object> Refresh(string username, string email);
        Task<UserData> GetUserInfo(ClaimsPrincipal email);
    }
}
