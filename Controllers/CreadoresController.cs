using Crud.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Models;
using AutoMapper;
using Crud.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreadoresController : ControllerBase
    {

        private readonly IIdentityService _identityService;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly EstadisticaCreadorService _estCreadorService;
        private readonly EstadisticaPlataformaService _estPlataformaService;

        public CreadoresController(
            ApplicationDbContext context, 
            IMapper mapper,
            IIdentityService identityService,
            EstadisticaCreadorService estCreadorService,
            EstadisticaPlataformaService estPlataformaService
            )
        {
            _identityService = identityService;
            _context = context;
            _mapper = mapper;
            _estCreadorService = estCreadorService;
            _estPlataformaService = estPlataformaService;
        }

        // GET: api/<CreadoresController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserData>>> GetCreadores()
        {
            var listCreatorsUsers = _mapper.Map<List<UserData>>(
                await _context.Creadores.Include(s => s.Usuario.Creador).Select(c => c.Usuario).ToListAsync()
            );
            return Ok(listCreatorsUsers);
        }

        // GET: api/<CreadoresController>
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Object>>> GetCreadoresSearch(string? search)
        {
            var creadores = _context.Creadores
                               .Include(c => c.Usuario)
                                .Select(item => new
                                {
                                    userid = item.UserId,
                                    username = item.Usuario.UserName,
                                    name = item.Usuario.Name,
                                    surname = item.Usuario.Surname,
                                });


            if (!String.IsNullOrEmpty(search))
            {
                creadores = _context.Creadores
                               .Where(c => (c.Usuario.UserName.Contains(search) ||
                                            c.Usuario.Name.Contains(search) ||
                                            c.Usuario.Surname.Contains(search)))
                               .Include(c => c.Usuario)
                                .Select(item => new
                                {
                                    userid = item.UserId,
                                    username = item.Usuario.UserName,
                                    name = item.Usuario.Name,
                                    surname = item.Usuario.Surname,
                                });

            }

            return await creadores.ToListAsync();

        }

        // GET api/<CreadoresController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserData>> GetCreador(string id)
        {
            var creador = _mapper.Map<UserData>(
                await _context.Usuarios
                .Where(c => c.Id == id)
                .Include(s => s.Creador)
                .Include(c => c.Creador.Categoria1)
                .Include(c => c.Creador.Categoria2)
                .Include(c => c.Creador.TiposDeSuscripciones)
                .FirstOrDefaultAsync()
                );

            if (creador == null)
            {
                return NotFound();
            }
            try
            {
                var user = await _identityService.GetUserInfo(HttpContext.User);


                if (user != null)
                {
                    var userModel = await _context.Users.Include(u => u.Siguiendo).FirstOrDefaultAsync(u => u.Id == user.Id);
                    var seguidor = userModel.Siguiendo.Where(u => u.CreadorId == id).FirstOrDefault();
                    if (seguidor != null)
                    {
                        creador.Creador.esSeguido = userModel.Siguiendo.Where(u => u.CreadorId == id).FirstOrDefault() != null;
                    }
                }
            }
            catch (Exception) { }
            
            try
            {
                _estPlataformaService.AddVisitaPerfil();
                _estCreadorService.AddVisitaPerfil(id);
            }
            catch (Exception){}

            return creador;
        }


        // POST api/<CreadoresController>
        [HttpPost("register")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> PostCreador(CreadorRegister creadorRegister)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            var creador = new Creador
            {
                Descripcion = creadorRegister.Descripcion,
                Imagen = creadorRegister.Imagen,
                ImagePortada = creadorRegister.ImagePortada,
                Biografia = creadorRegister.Biografia,
                VideoYoutube = creadorRegister.VideoYoutube,
                MsjBienvenidaGral = creadorRegister.MsjBienvenidaGral,
                Categoria1 = _context.Categoria.Find(creadorRegister.Categoria1Id),
                Categoria2 = _context.Categoria.Find(creadorRegister.Categoria2Id),
                UserId = user.Id,
                Id = user.Id,
            };

            int tipoId = int.Parse(_context.Parametros.Find("SUSCDEFECTO1").Valor);
            var tipo1 = _context.TipoSuscripcion.Find(tipoId);
            tipoId = int.Parse(_context.Parametros.Find("SUSCDEFECTO2").Valor);
            var tipo2 = _context.TipoSuscripcion.Find(tipoId);
            tipoId = int.Parse(_context.Parametros.Find("SUSCDEFECTO3").Valor);
            var tipo3 = _context.TipoSuscripcion.Find(tipoId);
            var newtipo1 = new TipoSuscripcion
            {
                CreadorId = user.Id,
                Activo = true,
                Beneficios = tipo1.Beneficios,
                Imagen = tipo1.Imagen,
                MensajeBienvenida = tipo1.MensajeBienvenida,
                MensajeriaActiva = tipo1.MensajeriaActiva,
                Nombre = tipo1.Nombre,
                Precio = tipo1.Precio,
            };
            var newtipo2 = new TipoSuscripcion
            {
                CreadorId = user.Id,
                Activo = true,
                Beneficios = tipo2.Beneficios,
                Imagen = tipo2.Imagen,
                MensajeBienvenida = tipo2.MensajeBienvenida,
                MensajeriaActiva = tipo2.MensajeriaActiva,
                Nombre = tipo2.Nombre,
                Precio = tipo2.Precio,
            };
            var newtipo3 = new TipoSuscripcion
            {
                CreadorId = user.Id,
                Activo = true,
                Beneficios = tipo3.Beneficios,
                Imagen = tipo3.Imagen,
                MensajeBienvenida = tipo3.MensajeBienvenida,
                MensajeriaActiva = tipo3.MensajeriaActiva,
                Nombre = tipo3.Nombre,
                Precio = tipo3.Precio,
            };

            creador.TiposDeSuscripciones = new List<TipoSuscripcion>
            {
                newtipo1,
                newtipo2,
                newtipo3,
            };

            _context.Creadores.Add(creador);
            _context.SaveChanges();

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

        // PUT api/<CreadoresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CreadoresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
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
            var userModel =await  _context.Users.Include(u => u.Siguiendo).Where(u => u.Id == user.Id).FirstOrDefaultAsync(); ;
            userModel.Siguiendo.Add(userCreador);

            _context.SaveChanges();

            try
            {
                 _estCreadorService.AddSeguidor(id);
                 _estPlataformaService.AddSeguidor();
            }
            catch (Exception){}

            return Ok();
        }


        [HttpDelete("follow")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UnfollowCreator(string id)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            if (user == null)
            {
                return BadRequest(HttpContext.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            }

            var userModel = _context.Users.Include(u => u.Siguiendo).FirstOrDefault(u => u.Id == user.Id);
            var userCreador = userModel.Siguiendo.FirstOrDefault(u => u.CreadorId == id);
            userModel.Siguiendo.Remove(userCreador);

            try
            {
                 _estCreadorService.SubSeguidor(id);
                 _estPlataformaService.SubSeguidor();
            }
            catch (Exception){}

            _context.SaveChanges();

            return Ok();
        }

    }
}
