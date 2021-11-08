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
    public class ParametrosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ParametrosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Parametros
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Parametros>>> GetParametros()
        {
            return await _context.Parametros.ToListAsync();
        }

        // GET: api/Parametros/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Parametros>> GetParametros(string id)
        {
            var parametros = await _context.Parametros.FindAsync(id);

            if (parametros == null)
            {
                return NotFound();
            }

            return parametros;
        }

        // PUT: api/Parametros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParametros(string id, Parametros parametros)
        {
            if (id != parametros.Nombre)
            {
                return BadRequest();
            }

            _context.Entry(parametros).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParametrosExists(id))
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

        // POST: api/Parametros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Parametros>> PostParametros(Parametros parametros)
        {
            _context.Parametros.Add(parametros);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ParametrosExists(parametros.Nombre))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetParametros", new { id = parametros.Nombre }, parametros);
        }

        // DELETE: api/Parametros/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParametros(string id)
        {
            var parametros = await _context.Parametros.FindAsync(id);
            if (parametros == null)
            {
                return NotFound();
            }

            _context.Parametros.Remove(parametros);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParametrosExists(string id)
        {
            return _context.Parametros.Any(e => e.Nombre == id);
        }
    }
}
