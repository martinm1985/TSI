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
    public class AudioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public AudioController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Audio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Audio>>> GetAudio()
        {
            return await _context.Audio.ToListAsync();
        }

        // GET: api/Audio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Audio>> GetAudio(int id)
        {
            var audio = await _context.Audio.FindAsync(id);

            if (audio == null)
            {
                return NotFound();
            }

            return audio;
        }

        // PUT: api/Audio/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAudio(int id, Audio audio)
        {
            if (id != audio.Id)
            {
                return BadRequest();
            }

            _context.Entry(audio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AudioExists(id))
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

        // POST: api/Audio
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Audio>> PostAudio(Audio audio)
        {
            _context.Audio.Add(audio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAudio", new { id = audio.Id }, audio);
        }

        // DELETE: api/Audio/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAudio(int id)
        {
            var audio = await _context.Audio.FindAsync(id);
            if (audio == null)
            {
                return NotFound();
            }

            _context.Audio.Remove(audio);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AudioExists(int id)
        {
            return _context.Audio.Any(e => e.Id == id);
        }
    }
}
