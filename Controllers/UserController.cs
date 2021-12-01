using Crud.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Crud.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Linq;
using AutoMapper;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using Crud.Data;
using Crud.Models;
using Microsoft.EntityFrameworkCore;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Crud.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;


        public UserController(IIdentityService identityService, ApplicationDbContext context, IMapper mapper)
        {
            _identityService = identityService;
            _context = context;
            _mapper = mapper;


        }

        [HttpPost("follow")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> FollowCreator(string id)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);


            if (user == null || user.Id == id)
            {
                return BadRequest(HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }

            var userCreador = new UserCreador
            {

                UserId = user.Id,
                CreadorId = id
            };

            var userModel = _context.Users.Include(u=> u.Siguiendo).FirstOrDefault(u => u.Id == user.Id);
            userModel.Siguiendo.Add(userCreador);

            _context.SaveChanges();


            return Ok();
        }


        [HttpDelete("follow")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UnfollowCreator(string id)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);


            if (user == null )
            {
                return BadRequest(HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }

        

            var userModel = _context.Users.Include(u => u.Siguiendo).FirstOrDefault(u => u.Id == user.Id);
            var userCreador = userModel.Siguiendo.FirstOrDefault(u => u.CreadorId == id);
            userModel.Siguiendo.Remove(userCreador);

            _context.SaveChanges();


            return Ok();
        }

        [HttpGet("following")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<Object>>> GetFollowingList()
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);


            if (user == null)
            {
                return BadRequest(HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }

            var followingList = _context.Users
                .Include(s => s.Siguiendo)
                .FirstOrDefault(u => u.Id == user.Id)
                .Siguiendo
                .Select(item => item.CreadorId)
                .ToList();

            var creatorsList = followingList
                .Select(id =>
                    _context.Creadores
                    .Where(u => u.Id == id)
                    .Include(c => c.Usuario)
                    .Select(item => new
                    {
                        userid = item.UserId,
                        username = item.Usuario.UserName,
                        name = item.Usuario.Name,
                        surname = item.Usuario.Surname,
                    }).FirstOrDefault()
                    );



            return Ok(creatorsList);
        }

        [HttpGet("followers")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<IEnumerable<Object>>> GetFollowersList()
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            
            if (user == null || user.Creador == null)
            {
                return BadRequest(HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }

            var followingList = _context.Creadores
                .Include(s => s.Seguidores)
                .FirstOrDefault(u => u.Id == user.Id)
                .Seguidores
                .Select(item => item.UserId)
                .ToList();

            var creatorsList = followingList
                .Select(id =>
                    _context.Usuarios
                    .Where(u => u.Id == id)
                    .Select(item => new
                    {
                        userid = item.Id,
                        username = item.UserName,
                        name = item.Name,
                        surname = item.Surname,
                    }).FirstOrDefault()
                    );



            return Ok(creatorsList);
        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateProfile(UserData request)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            try
            {
                var userData = _context.Users.Find(user.Id);
                userData.Name = request.Name;
                userData.Surname = request.Surname;
                userData.UserName = request.Username;
                _context.Entry(userData).State = EntityState.Modified;
                if (user.Creador != null)
                {
                    var creador = _context.Creadores.Find(user.Creador.Id);
                    creador.MsjBienvenidaGral = request.Creador.MsjBienvenidaGral;
                    creador.VideoYoutube = request.Creador.VideoYoutube;
                    creador.Biografia = request.Creador.Biografia;
                    creador.Descripcion = request.Creador.Descripcion;
                    creador.Categoria1 = _context.Categoria.Find(request.Creador.Categoria1Id);
                    creador.Categoria2 = _context.Categoria.Find(request.Creador.Categoria2Id);
                    _context.Entry(creador).State = EntityState.Modified;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest(HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }

            return Ok(_mapper.Map<UserData>(
                await _context.Usuarios
                .Where(c => c.Id == user.Id)
                .Include(s => s.Creador)
                .Include(c => c.Creador.Categoria1)
                .Include(c => c.Creador.Categoria2)
                .Include(c => c.Creador.TiposDeSuscripciones)
                .FirstOrDefaultAsync()
                ));

        }

      

    }
}
