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
using Microsoft.AspNetCore.SignalR;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MensajeApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private IHubContext<ChatHub> _hubContext;

        public MensajeApiController(ApplicationDbContext context, IIdentityService identityService, IHubContext<ChatHub> hubContext)
        {
            _context = context;
            _identityService = identityService;
            _hubContext = hubContext;
        }

        // GET: api/MensajeApi
        [HttpGet("list/{conversacionId}")]
        public async Task<ActionResult<IEnumerable<ConversacionDto.MensajesGet>>> GetMensajeConversacion(int conversacionId, int page, int pageSize)
        {
            return await   _context.Mensaje
                            .Where(m => m.ConversacionId == conversacionId)
                            .OrderByDescending(m => m.FechaHora)
                            .Select(m => new ConversacionDto.MensajesGet
                            {
                                MensajeId = m.MensajeId,
                                DateTimeSent = m.FechaHora.ToShortDateString() + " " +  m.FechaHora.ToShortTimeString(),
                                Body = m.CuerpoMensaje,
                                Read = m.Leido,
                                ConversacionId = m.ConversacionId,
                                UserSender = m.UserSender,
                            })
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToListAsync();
        }

        // GET: api/MensajeApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ConversacionDto.MensajesGet>> GetMensaje(int id)
        {
            var mensaje = _context.Mensaje
                                    .Where(m => m.MensajeId == id)
                                    .Select(m => new ConversacionDto.MensajesGet
                                    {
                                        MensajeId = m.MensajeId,
                                        DateTimeSent = m.FechaHora.ToShortDateString() + " " + m.FechaHora.ToShortTimeString(),
                                        Body = m.CuerpoMensaje,
                                        Read = m.Leido,
                                        ConversacionId = m.ConversacionId,
                                        UserSender = m.UserSender,
                                    }).FirstOrDefault();

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

            if (user == null) return BadRequest();
          
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
            
            try
            {
                var conversacion = _context.Conversacion.Find(convId);
                await _hubContext.Clients.Group(conversacion.CreadorId).SendAsync("ReceiveMessage", msg);
                await _hubContext.Clients.Group(conversacion.UsuarioId).SendAsync("ReceiveMessage", msg);
            } catch { };


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
