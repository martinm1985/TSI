using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Models;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoSuscripcionesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TipoSuscripcionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/TipoSuscripciones
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TipoSuscripcion>>> GetTipoSuscripcion()
        {
            return await _context.TipoSuscripcion.ToListAsync();
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
        public async Task<IActionResult> PutTipoSuscripcion(int id, TipoSuscripcion tipoSuscripcion)
        {
            if (id != tipoSuscripcion.Id)
            {
                return BadRequest();
            }

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
