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
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;

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

        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] string email) {
            await _identityService.ForgotPassword(email);
            return Ok("Email sent");
        }

        [HttpPatch("{id}/resetpassword")]
        public async Task<IActionResult> ResetPassword(string id, PasswrodResetRequest request)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null) {
                return NotFound();
            }
            var isTokenValid = await _userManager.VerifyUserTokenAsync(
                user,
                _userManager.Options.Tokens.PasswordResetTokenProvider,
                "ResetPassword",
                request.Token
            );

            if (!isTokenValid)
            {
                return BadRequest("Link expired");
            }

            var passwordValidator = new PasswordValidator<User>();
            var result = await passwordValidator.ValidateAsync(_userManager, user, request.Password);

            if (!result.Succeeded) {
                return BadRequest("Password not valid");
            }

            
            await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            return Ok("Password Reseted successfully");
            
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login (LoginRequest request)
        {
            var res = new object();

            if (request.FacebookToken == null)
            {
                res = await _identityService.Login(request.Username, request.Password, false, null);
            }
            else
            {
                using var client = new HttpClient();

                var url = "https://graph.facebook.com/me?fields=email&access_token=" + request.FacebookToken;

                var result = await client.GetAsync(url);

                var contents = await result.Content.ReadAsStringAsync();

                FacebookResponse json = JsonConvert.DeserializeObject<FacebookResponse>(contents);

                Console.WriteLine(json.Id);
                Console.WriteLine(json.Email);
                res = await _identityService.Login(json.Email, null, true, json.Id);
            }

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

        [HttpPost("blockUser/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "AdminAuthorization")]
        public async Task<ActionResult> BlockUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            var isLockoutEnabled = await _userManager.GetLockoutEnabledAsync(user);

            if (!isLockoutEnabled)
            {
               
                return BadRequest("El usuario no puede ser bloqueado");
            }

            var isBlocked = user.LockoutEnd != null &&
                            DateTimeOffset.Compare((DateTimeOffset)user.LockoutEnd, DateTimeOffset.UtcNow) > 0;

            if (isBlocked)
            {
                await _userManager.SetLockoutEndDateAsync(user, null);
            }
            else
            {
                await _userManager.SetLockoutEndDateAsync(user, DateTime.Now.AddYears(100));
            }
            return Ok();
        }

    }
}
