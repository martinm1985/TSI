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
    public class SuscripcionUsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SuscripcionUsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SuscripcionUsuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SuscripcionUsuario>>> GetSuscripcionUsuario()
        {
            return await _context.SuscripcionUsuario.ToListAsync();
        }

        // GET: api/SuscripcionUsuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SuscripcionUsuario>> GetSuscripcionUsuario(int id)
        {
            var suscripcionUsuario = await _context.SuscripcionUsuario.FindAsync(id);

            if (suscripcionUsuario == null)
            {
                return NotFound();
            }

            return suscripcionUsuario;
        }

        // PUT: api/SuscripcionUsuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSuscripcionUsuario(int id, SuscripcionUsuario suscripcionUsuario)
        {
            if (id != suscripcionUsuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(suscripcionUsuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SuscripcionUsuarioExists(id))
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

        // POST: api/SuscripcionUsuarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SuscripcionUsuario>> PostSuscripcionUsuario(SuscripcionUsuario suscripcionUsuario)
        {
            _context.SuscripcionUsuario.Add(suscripcionUsuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSuscripcionUsuario", new { id = suscripcionUsuario.Id }, suscripcionUsuario);
        }

        // DELETE: api/SuscripcionUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuscripcionUsuario(int id)
        {
            var suscripcionUsuario = await _context.SuscripcionUsuario.FindAsync(id);
            if (suscripcionUsuario == null)
            {
                return NotFound();
            }

            _context.SuscripcionUsuario.Remove(suscripcionUsuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SuscripcionUsuarioExists(int id)
        {
            return _context.SuscripcionUsuario.Any(e => e.Id == id);
        }
    }
}
