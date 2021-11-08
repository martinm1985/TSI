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
    public class TextoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public TextoController(ApplicationDbContext context)
        {
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
        public async Task<ActionResult<Texto>> GetTexto(int id)
        {
            var texto = await _context.Texto.FindAsync(id);

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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Texto>> PostTexto(Texto texto)
        {
            _context.Texto.Add(texto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTexto", new { id = texto.Id }, texto);
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
