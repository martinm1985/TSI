using Crud.DTOs;
using Crud.Models;
using Crud.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;

namespace Crud.Controllers
{
    [Route("api/identity")]
    [ApiController]
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;


        private readonly UserManager<User> _userManager;

        public IdentityController(IIdentityService identityService, UserManager<User> userManager)
        {
            this._identityService = identityService;
            _userManager = userManager;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register (UserRegister request)
        {

            var res = await _identityService.RegisterUser(request.Username, request.Email, request.Password, request.Name, request.Surname);

            if (res.GetType() == typeof(ResponseFailure))
            {
                return BadRequest(res);
            }

            return Ok(res);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginRequest request)
        {
            var res = await _identityService.Login(request.Username, request.Password);

            if (res.GetType() == typeof(ResponseFailure))
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequest request)
        {
            var res = await _identityService.Refresh(request.RefreshToken, request.Token);

            if (res.GetType() == typeof(ResponseFailure))
            {
                return BadRequest(res);
            }

            return Ok(res);
        }

        [HttpPost("make_user_admin")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme ,Policy = "AdminAuthorization")]
        public async Task<ActionResult<UserData>> MakeUserAdmin(string userId)
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            if (admin.Id == userId) {
                 return BadRequest("Un usuario no puede hacerse admin a si mismo");
             }

            var res = await _identityService.MakeAdmin(userId);

            return res != null ? Ok(res) : BadRequest("No existe un usuario con esa id");
        }

        [HttpPost("remove_admin_user")]
        [Authorize(Policy = "AdminAuthorization")]
        public async Task<ActionResult<UserData>> RemoveAdminUser(string userId)
        {
            var admin = await _userManager.GetUserAsync(HttpContext.User);

            if (admin.Id == userId) {
                 return BadRequest("Un usuario no puede sacarse sus permisos de admin");
             }


            var res = await _identityService.RemoveAdmin(userId);

            return res != null ? Ok(res) : BadRequest("No existe un usuario con esa id");;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Identity()
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            if (user == null)
            {
                return BadRequest(HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }

            return Ok(user);
        }

    }
}
