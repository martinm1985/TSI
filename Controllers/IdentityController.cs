using Crud.DTOs;
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

        public IdentityController(IIdentityService identityService)
        {
            this._identityService = identityService;
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