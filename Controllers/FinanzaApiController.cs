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
    public class FinanzaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FinanzaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FinanzaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Finanza>>> GetFinanza()
        {
            return await _context.Finanza.ToListAsync();
        }

        // GET: api/FinanzaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Finanza>> GetFinanza(int id)
        {
            var finanza = await _context.Finanza.FindAsync(id);

            if (finanza == null)
            {
                return NotFound();
            }

            return finanza;
        }

        // PUT: api/FinanzaApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinanza(int id, Finanza finanza)
        {
            if (id != finanza.FinanzaId)
            {
                return BadRequest();
            }

            _context.Entry(finanza).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinanzaExists(id))
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

        // POST: api/FinanzaApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Finanza>> PostFinanza(Finanza finanza)
        {
            _context.Finanza.Add(finanza);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinanza", new { id = finanza.FinanzaId }, finanza);
        }

        // DELETE: api/FinanzaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinanza(int id)
        {
            var finanza = await _context.Finanza.FindAsync(id);
            if (finanza == null)
            {
                return NotFound();
            }

            _context.Finanza.Remove(finanza);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinanzaExists(int id)
        {
            return _context.Finanza.Any(e => e.FinanzaId == id);
        }
    }
}
