using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Models;
using Crud.DTOs;
using Crud.Services;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public MensajeApiController(ApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        // GET: api/MensajeApi
        [HttpGet("list/{conversacionId}")]
        public async Task<ActionResult<IEnumerable<ConversacionDto.MensajesGet>>> GetMensajeConversacion(int conversacionId)
        {
            return await   _context.Mensaje
                            .Where(m => m.ConversacionId == conversacionId)
                            .OrderBy(m => m.FechaHora)
                            .Select(m => new ConversacionDto.MensajesGet
                            {
                                MensajeId = m.MensajeId,
                                DateTimeSent = m.FechaHora.ToShortDateString() + " " +  m.FechaHora.ToShortTimeString(),
                                Body = m.CuerpoMensaje,
                                Read = m.Leido,
                                ConversacionId = m.ConversacionId,
                                UserSender = m.UserSender,
                            })
                            .ToListAsync();
        }

        // GET: api/MensajeApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Mensaje>> GetMensaje(int id)
        {
            var mensaje = await _context.Mensaje.FindAsync(id);

            if (mensaje == null)
            {
                return NotFound();
            }

            return mensaje;
        }

        // PUT: api/MensajeApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMensaje(int id, Mensaje mensaje)
        {
            if (id != mensaje.MensajeId)
            {
                return BadRequest();
            }

            _context.Entry(mensaje).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MensajeExists(id))
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

        // POST: api/MensajeApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ConversacionDto.MensajeAdd>> PostMensaje(ConversacionDto.MensajeAdd mensaje)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
          
            int convId = mensaje.ConversacionId;

            if (mensaje.ConversacionId == 0)
            {
                if (mensaje.CreadorId != null || mensaje.CreadorId != "")
                {
                    var convFind = _context.Conversacion
                                  .Where(c => c.CreadorId == mensaje.CreadorId)
                                  .Where(c => c.UsuarioId == user.Id)
                                  .FirstOrDefault();

                    if (convFind != null)
                    {
                        // Existe una conversacion 
                        convId = convFind.ConversacionId;
                    }
                    else
                    {
                        // Si no hay conversacion, se crea para poner el mensaje
                        var conv = new Conversacion
                        {
                            UsuarioId = user.Id,
                            CreadorId = mensaje.CreadorId,
                            FechaInicio = DateTime.UtcNow.Date,
                        };
                        _context.Conversacion.Add(conv);
                        await _context.SaveChangesAsync();
                        convId = conv.ConversacionId;
                    }

                } else
                {
                    return BadRequest();
                }
               
            }

            var msg = new Mensaje
            {
                FechaHora = DateTime.Now,
                CuerpoMensaje = mensaje.Body,
                Leido = false,
                ConversacionId = convId,
                UserSender = user.Id,
            };
            _context.Mensaje.Add(msg);
            await _context.SaveChangesAsync();

            return Ok(msg);
        }

        // DELETE: api/MensajeApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMensaje(int id)
        {
            var mensaje = await _context.Mensaje.FindAsync(id);
            if (mensaje == null)
            {
                return NotFound();
            }

            _context.Mensaje.Remove(mensaje);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MensajeExists(int id)
        {
            return _context.Mensaje.Any(e => e.MensajeId == id);
        }
    }
}
