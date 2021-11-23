using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Models;
using Crud.DTOs;
using Crud.Services;


namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiveStreamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public LiveStreamController(ApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }

        // GET: api/LiveStream
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiveStream>>> GetLiveStream()
        {
            return await _context.LiveStream.ToListAsync();
        }

        // GET: api/LiveStream/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetLiveStream(int id)
        {
            var liveStream = _context.LiveStream
                         .Include(c => c.Categoria)
                         .Include(c => c.Creador)
                         .Where(c => c.Id == id)
                         .Select(item => new
                         {
                             id = item.Id,
                             username = item.Creador.Usuario.UserName,
                             titulo = item.Titulo,
                             descripcion = item.Descripcion,
                             fechaCreacion = item.FechaCreacion.ToShortDateString(),
                             categoriaId = item.CategoriaId,
                             categoriaNombre = item.Categoria.Nombre,
                             texto = "",
                             largo = 0,
                             archivo = "",
                             calidad = "",
                             duracion = 0,
                             duracionVideo = 0,
                             url = "",
                             fechaInicio = item.FechaInicio.ToShortDateString(),
                             fechaFin = item.FechaFin.ToShortDateString(),
                         }).FirstOrDefault();

            if (liveStream == null)
            {
                return NotFound();
            }

            return liveStream;

        }

        // PUT: api/LiveStream/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLiveStream(int id, LiveStream liveStream)
        {
            if (id != liveStream.Id)
            {
                return BadRequest();
            }

            _context.Entry(liveStream).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStreamExists(id))
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

        // POST: api/LiveStream
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LiveStream>> PostLiveStream(ContenidoDto.ContenidoRegistro contenido)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            var liveStream = new LiveStream
            {
                CreadorId = user.Id,
                Titulo = contenido.Titulo,
                Descripcion = contenido.Descripcion,
                FechaCreacion = DateTime.Now.Date,
                Bloqueado = false,
                DerechoAutor = contenido.DerechoAutor,
                //Archivo = contenido.Archivo,
                //Calidad = contenido.Calidad,
                CategoriaId = contenido.CategoriaId,
                TipoSuscripcionId = contenido.TipoSuscripcionId,
                FechaInicio = contenido.FechaInicio,
                FechaFin = contenido.FechaFin
            };

            _context.LiveStream.Add(liveStream);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/LiveStream/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiveStream(int id)
        {
            var liveStream = await _context.LiveStream.FindAsync(id);
            if (liveStream == null)
            {
                return NotFound();
            }

            _context.LiveStream.Remove(liveStream);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiveStreamExists(int id)
        {
            return _context.LiveStream.Any(e => e.Id == id);
        }
    }
}
