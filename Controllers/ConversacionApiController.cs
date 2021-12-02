using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Crud.Data;
using Crud.Models;
using Crud.Services;
using Crud.DTOs;


namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversacionApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ConversacionApiController(ApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        // GET: api/ConversacionApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ConversacionDto.ConversacionGet>>> GetConversacion(string? search)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            var conversacion = _context.Conversacion
                               .Include(c => c.Creador.Usuario)
                               .Where(c => ((user.Creador == null && c.UsuarioId == user.Id) || 
                                            (user.Creador != null && c.CreadorId == user.Creador.Id)))
                                .Where(c => (String.IsNullOrEmpty(search) ||
                                                (c.Creador.Usuario.UserName.Contains(search) ||
                                                c.Creador.Usuario.Name.Contains(search) ||
                                                c.Creador.Usuario.Surname.Contains(search))
                                       ))
                                .Select(item => new ConversacionDto.ConversacionGet
                                {
                                    Id = item.ConversacionId,
                                    CreadorId = item.CreadorId,
                                    UserId = item.UsuarioId,
                                    Username = item.Creador.Usuario.UserName,
                                    MensageId = 0,
                                    Read = true,
                                }).ToList();

            foreach (var item in conversacion)
            {
                var mensaje = _context.Mensaje
                                .Where(m => m.ConversacionId == item.Id)
                                .OrderByDescending(m => m.FechaHora)
                                .FirstOrDefault();
                if (mensaje != null)
                {
                    item.MensageId = mensaje.MensajeId;
                    item.Body = mensaje.CuerpoMensaje;
                    item.FechaHora = mensaje.FechaHora.ToString();
                    item.FechaHoraOrden = mensaje.FechaHora;
                    item.Read = mensaje.Leido;
                    item.UserSender = mensaje.UserSender;
                }
            }

            conversacion = conversacion.OrderByDescending(m => m.FechaHoraOrden).ToList();

            return Ok(conversacion);

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
