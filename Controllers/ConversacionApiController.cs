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
    public class ConversacionApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ConversacionApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ConversacionApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Conversacion>>> GetConversacion()
        {
            return await _context.Conversacion.ToListAsync();
        }

        // GET: api/ConversacionApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Conversacion>> GetConversacion(int id)
        {
            var conversacion = await _context.Conversacion.FindAsync(id);

            if (conversacion == null)
            {
                return NotFound();
            }

            return conversacion;
        }

        // PUT: api/ConversacionApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutConversacion(int id, Conversacion conversacion)
        {
            if (id != conversacion.ConversacionId)
            {
                return BadRequest();
            }

            _context.Entry(conversacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConversacionExists(id))
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

        // POST: api/ConversacionApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Conversacion>> PostConversacion(Conversacion conversacion)
        {
            _context.Conversacion.Add(conversacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetConversacion", new { id = conversacion.ConversacionId }, conversacion);
        }

        // DELETE: api/ConversacionApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteConversacion(int id)
        {
            var conversacion = await _context.Conversacion.FindAsync(id);
            if (conversacion == null)
            {
                return NotFound();
            }

            _context.Conversacion.Remove(conversacion);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ConversacionExists(int id)
        {
            return _context.Conversacion.Any(e => e.ConversacionId == id);
        }
    }
}
