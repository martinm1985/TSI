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
    public class FinanzaPlataformaApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FinanzaPlataformaApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/FinanzaPlataformaApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinanzaPlataforma>>> GetFinanzaPlataforma()
        {
            return await _context.FinanzaPlataforma.ToListAsync();
        }

        // GET: api/FinanzaPlataformaApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinanzaPlataforma>> GetFinanzaPlataforma(int id)
        {
            var finanzaPlataforma = await _context.FinanzaPlataforma.FindAsync(id);

            if (finanzaPlataforma == null)
            {
                return NotFound();
            }

            return finanzaPlataforma;
        }

        // PUT: api/FinanzaPlataformaApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFinanzaPlataforma(int id, FinanzaPlataforma finanzaPlataforma)
        {
            if (id != finanzaPlataforma.FinanzaPlataformaId)
            {
                return BadRequest();
            }

            _context.Entry(finanzaPlataforma).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FinanzaPlataformaExists(id))
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

        // POST: api/FinanzaPlataformaApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FinanzaPlataforma>> PostFinanzaPlataforma(FinanzaPlataforma finanzaPlataforma)
        {
            _context.FinanzaPlataforma.Add(finanzaPlataforma);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinanzaPlataforma", new { id = finanzaPlataforma.FinanzaPlataformaId }, finanzaPlataforma);
        }

        // DELETE: api/FinanzaPlataformaApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFinanzaPlataforma(int id)
        {
            var finanzaPlataforma = await _context.FinanzaPlataforma.FindAsync(id);
            if (finanzaPlataforma == null)
            {
                return NotFound();
            }

            _context.FinanzaPlataforma.Remove(finanzaPlataforma);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FinanzaPlataformaExists(int id)
        {
            return _context.FinanzaPlataforma.Any(e => e.FinanzaPlataformaId == id);
        }
    }
}
