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
    public class LiveStreamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LiveStreamController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/LiveStream
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LiveStream>>> GetLiveStream()
        {
            return await _context.LiveStream.ToListAsync();
        }

        // GET: api/LiveStream/5
        [HttpGet("{id}")]
        public async Task<ActionResult<LiveStream>> GetLiveStream(int id)
        {
            var liveStream = await _context.LiveStream.FindAsync(id);

            if (liveStream == null)
            {
                return NotFound();
            }

            return liveStream;
        }

        // PUT: api/LiveStream/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLiveStream(int id, LiveStream liveStream)
        {
            if (id != liveStream.Id)
            {
                return BadRequest();
            }

            _context.Entry(liveStream).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LiveStreamExists(id))
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

        // POST: api/LiveStream
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<LiveStream>> PostLiveStream(LiveStream liveStream)
        {
            _context.LiveStream.Add(liveStream);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLiveStream", new { id = liveStream.Id }, liveStream);
        }

        // DELETE: api/LiveStream/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLiveStream(int id)
        {
            var liveStream = await _context.LiveStream.FindAsync(id);
            if (liveStream == null)
            {
                return NotFound();
            }

            _context.LiveStream.Remove(liveStream);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LiveStreamExists(int id)
        {
            return _context.LiveStream.Any(e => e.Id == id);
        }
    }
}
