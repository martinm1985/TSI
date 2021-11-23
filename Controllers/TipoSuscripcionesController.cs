using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.DTOs;
using Crud.Models;
using Crud.Services;
using static Crud.DTOs.SuscripcionDto;
using AutoMapper;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoSuscripcionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;

        public TipoSuscripcionesController(ApplicationDbContext context, IIdentityService identityService, IMapper mapper)
        {
            _identityService = identityService;
            _context = context;
            _mapper = mapper;
        }

        // GET: api/TipoSuscripciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoSuscripcion>>> GetTipoSuscripcion()
        {
            return await _context.TipoSuscripcion.ToListAsync();
        }

        // GET: api/TipoSuscripcionesDefecto
        [HttpGet("defecto")]
        public async Task<ActionResult<IEnumerable<Object>>> GetTipoSuscripcionDefecto()
        {

            var susc = _context.Parametros
                        .Where(p => (p.Nombre == "SUSCDEFECTO1" || p.Nombre == "SUSCDEFECTO2" || p.Nombre == "SUSCDEFECTO3"))
                        .Select(p => int.Parse(p.Valor)).ToList();

            if (susc == null)
            {
                return BadRequest();
            }
            
            var result = from t in _context.TipoSuscripcion
                         where susc.Contains(t.Id)
                         select t; 
            
            return result.ToList();
        }

        // GET: api/TipoSuscripcionesCreador
        [HttpGet("creador")]
        public async Task<ActionResult<IEnumerable<Object>>> GetTipoSuscripcionCreador()
        {
            try
            {
                var user = await _identityService.GetUserInfo(HttpContext.User);

                if (user != null)
                {
                    var tipoSuscripcion = _mapper.Map<List<TipoSuscripcionDto>>(
                         await _context.TipoSuscripcion
                                .Where(t => t.CreadorId == user.Id)
                                .Where(t => t.Activo).ToListAsync());

                    return Ok(tipoSuscripcion);
                }
            }
            catch
            {
                return null;
            }
            return null;
           
        }

        // GET: api/TipoSuscripciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TipoSuscripcion>> GetTipoSuscripcion(int id)
        {
            var tipoSuscripcion = await _context.TipoSuscripcion.FindAsync(id);

            if (tipoSuscripcion == null)
            {
                return NotFound();
            }

            return tipoSuscripcion;
        }

        // PUT: api/TipoSuscripciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTipoSuscripcion(int id, SuscripcionDto.TipoSuscripcionDto request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }

            var tipoSuscripcion = _context.TipoSuscripcion.Find(id);
            tipoSuscripcion.Nombre = request.Nombre;
            tipoSuscripcion.Precio = request.Precio;
            tipoSuscripcion.Activo = request.Activo;
            tipoSuscripcion.Beneficios = request.Beneficios;
            tipoSuscripcion.Imagen = request.Imagen;
            tipoSuscripcion.MensajeBienvenida = request.MensajeBienvenida;
            tipoSuscripcion.MensajeriaActiva = request.MensajeriaActiva;
            tipoSuscripcion.IncluyeTipoSuscrId = request.IncluyeTipoSuscrId;
            tipoSuscripcion.VideoBienvenida = request.VideoBienvenida;
            _context.Entry(tipoSuscripcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TipoSuscripcionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TipoSuscripciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TipoSuscripcion>> PostTipoSuscripcion(TipoSuscripcion tipoSuscripcion)
        {
            _context.TipoSuscripcion.Add(tipoSuscripcion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTipoSuscripcion", new { id = tipoSuscripcion.Id }, tipoSuscripcion);
        }

        // DELETE: api/TipoSuscripciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTipoSuscripcion(int id)
        {
            var tipoSuscripcion = await _context.TipoSuscripcion.FindAsync(id);
            if (tipoSuscripcion == null)
            {
                return NotFound();
            }

            _context.TipoSuscripcion.Remove(tipoSuscripcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TipoSuscripcionExists(int id)
        {
            return _context.TipoSuscripcion.Any(e => e.Id == id);
        }

        private async void TipoSuscripcionPorDefecto()
        {
            //TODO: chequear si existen sino return
            TipoSuscripcion tipoSuscripcion = new()
            {
                Creador = null,
                Nombre = "Basico",
                Precio = 1,
                Imagen = "TODO",
                Beneficios = "Acceso a un nuevo contenido por semana;;",
                MensajeBienvenida = "",
                VideoBienvenida = false,
                MensajeriaActiva = false,
                Activo = true,
                IncluyeTipoSuscrId = null,
            };

            _context.TipoSuscripcion.Add(tipoSuscripcion);
            await _context.SaveChangesAsync();

             TipoSuscripcion tipoSuscripcion2 = new()
            {
                Creador = null,
                Nombre = "Estandar",
                Precio = 5,
                Imagen = "TODO",
                Beneficios = "Acceso a todo el contenido subido;;",
                MensajeBienvenida = "",
                VideoBienvenida = false,
                MensajeriaActiva = false,
                Activo = true,
                IncluyeTipoSuscrId = tipoSuscripcion.Id,
            };

            _context.TipoSuscripcion.Add(tipoSuscripcion2);
            await _context.SaveChangesAsync();

            TipoSuscripcion tipoSuscripcion3 = new()
            {
                Creador = null,
                Nombre = "Estandar",
                Precio = 10,
                Imagen = "TODO",
                Beneficios = "Todo lo que incluye el estandar;;CHATEA CONMIGO;;",
                MensajeBienvenida = "",
                VideoBienvenida = false,
                MensajeriaActiva = true,
                Activo = true,
                IncluyeTipoSuscrId = tipoSuscripcion2.Id,
            };

            _context.TipoSuscripcion.Add(tipoSuscripcion3);
            await _context.SaveChangesAsync();

            //TODO: Guardar ids en la tabla parametros
        }
    }
}
