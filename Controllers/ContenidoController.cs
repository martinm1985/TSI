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
    public class ContenidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContenidoController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Contenido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contenido>>> GetContenido()
        {
            return Ok(await _context.Contenido.ToListAsync());
        }

        // GET: api/Contenido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contenido>> GetContenido(int id)
        {
            var contenido = await _context.Contenido.FindAsync(id);

            if (contenido == null)
            {
                return NotFound();
            }

            return contenido;
        }

        // PUT: api/Contenido/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContenido(int id, Contenido contenido)
        {
            if (id != contenido.Id)
            {
                return BadRequest();
            }

            _context.Entry(contenido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContenidoExists(id))
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

        // POST: api/Contenido
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contenido>> PostContenido(Contenido contenido)
        {
            _context.Contenido.Add(contenido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContenido", new { id = contenido.Id }, contenido);
        }

        // DELETE: api/Contenido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContenido(int id)
        {
            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null)
            {
                return NotFound();
            }

            _context.Contenido.Remove(contenido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContenidoExists(int id)
        {
            return _context.Contenido.Any(e => e.Id == id);
        }

        





    }
}
