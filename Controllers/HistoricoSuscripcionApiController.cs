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
    public class HistoricoSuscripcionApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public HistoricoSuscripcionApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/HistoricoSuscripcionApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HistoricoSuscripcion>>> GetHistoricoSuscripcion()
        {
            return await _context.HistoricoSuscripcion.ToListAsync();
        }

        // GET: api/HistoricoSuscripcionApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HistoricoSuscripcion>> GetHistoricoSuscripcion(int id)
        {
            var historicoSuscripcion = await _context.HistoricoSuscripcion.FindAsync(id);

            if (historicoSuscripcion == null)
            {
                return NotFound();
            }

            return historicoSuscripcion;
        }

        // PUT: api/HistoricoSuscripcionApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHistoricoSuscripcion(int id, HistoricoSuscripcion historicoSuscripcion)
        {
            if (id != historicoSuscripcion.HistoricoSuscripcionId)
            {
                return BadRequest();
            }

            _context.Entry(historicoSuscripcion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HistoricoSuscripcionExists(id))
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

        // POST: api/HistoricoSuscripcionApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HistoricoSuscripcion>> PostHistoricoSuscripcion(HistoricoSuscripcion historicoSuscripcion)
        {
            _context.HistoricoSuscripcion.Add(historicoSuscripcion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHistoricoSuscripcion", new { id = historicoSuscripcion.HistoricoSuscripcionId }, historicoSuscripcion);
        }

        // DELETE: api/HistoricoSuscripcionApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHistoricoSuscripcion(int id)
        {
            var historicoSuscripcion = await _context.HistoricoSuscripcion.FindAsync(id);
            if (historicoSuscripcion == null)
            {
                return NotFound();
            }

            _context.HistoricoSuscripcion.Remove(historicoSuscripcion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HistoricoSuscripcionExists(int id)
        {
            return _context.HistoricoSuscripcion.Any(e => e.HistoricoSuscripcionId == id);
        }
    }
}
