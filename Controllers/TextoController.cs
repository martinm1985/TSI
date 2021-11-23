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

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TextoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;


        public TextoController(ApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }

        // GET: api/Texto
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Texto>>> GetTexto()
        {
            return await _context.Texto.ToListAsync();
        }

        // GET: api/Texto/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetTexto(int id)
        {
            var texto = _context.Texto
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
                             texto = item.Html,
                             largo = item.Largo,
                             archivo = "",
                             calidad = "",
                             duracion = 0,
                             duracionVideo = 0,
                             url = "",
                             fechaInicio = "",
                             fechaFin = "",
                         }).FirstOrDefault();

            if (texto == null)
            {
                return NotFound();
            }

            return texto;
        }

        // PUT: api/Texto/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTexto(int id, Texto texto)
        {
            if (id != texto.Id)
            {
                return BadRequest();
            }

            _context.Entry(texto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TextoExists(id))
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

        // POST: api/Texto
        [HttpPost]
        public async Task<ActionResult<Texto>> PostTexto(ContenidoDto.ContenidoRegistro contenido)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            var texto = new Texto
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
                Largo = contenido.Largo,
                Html = contenido.Texto,
            };

            _context.Texto.Add(texto);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Texto/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTexto(int id)
        {
            var texto = await _context.Texto.FindAsync(id);
            if (texto == null)
            {
                return NotFound();
            }

            _context.Texto.Remove(texto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TextoExists(int id)
        {
            return _context.Texto.Any(e => e.Id == id);
        }
    }
}
