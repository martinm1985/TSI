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
    public class LinkController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public LinkController(ApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }

        // GET: api/Link
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Link>>> GetLink()
        {
            return await _context.Link.ToListAsync();
        }

        // GET: api/Link/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetLink(int id)
        {
            var link = _context.Link
                         .Include(c => c.Categoria)
                         .Include(c => c.Creador)
                         .Where(c => (c.Id == id))
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
                             url = item.Url,
                             fechaInicio = "",
                             fechaFin = "",
                         }).FirstOrDefault();

            if (link == null)
            {
                return NotFound();
            }

            return link;
        }

        // PUT: api/Link/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLink(int id, Link link)
        {
            if (id != link.Id)
            {
                return BadRequest();
            }

            _context.Entry(link).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LinkExists(id))
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

        // POST: api/Link
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Link>> PostLink(ContenidoDto.ContenidoRegistro contenido)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            var link = new Link
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
                Url = contenido.Url,
                
            };

            _context.Link.Add(link);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Link/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLink(int id)
        {
            var link = await _context.Link.FindAsync(id);
            if (link == null)
            {
                return NotFound();
            }

            _context.Link.Remove(link);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LinkExists(int id)
        {
            return _context.Link.Any(e => e.Id == id);
        }
    }
}
