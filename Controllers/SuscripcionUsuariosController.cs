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
using SendGrid;
using SendGrid.Helpers.Mail;
using AutoMapper;
using Crud.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SuscripcionUsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;
        private readonly IMapper _mapper;
        private readonly EstadisticaCreadorService _estCreadorService;
        private readonly EstadisticaPlataformaService _estPlataformaService;

        public SuscripcionUsuariosController(ApplicationDbContext context,
            IIdentityService identityService,
            IMapper mapper,
            EstadisticaCreadorService estCreadorService,
            EstadisticaPlataformaService estPlataformaService
            )
        {
            _context = context;
            _identityService = identityService;
            _mapper = mapper;
            _estCreadorService = estCreadorService;
            _estPlataformaService = estPlataformaService;

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
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<SuscripcionUsuario>> PostSuscripcionUsuario(SuscripcionUsuarioDto suscripcionUsuario)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            var creadorId = await _context.TipoSuscripcion.
                Where(m => m.Id == suscripcionUsuario.TipoSuscripcionId).
                Select(m => m.CreadorId).FirstOrDefaultAsync();

            if (user.Id != creadorId)
            {
                var subsUser = new SuscripcionUsuario
                {

                    FechaInicio = DateTime.Now,
                    FechaFin = suscripcionUsuario.FechaFin,
                    Activo = true,
                    TipoSuscripcionId = suscripcionUsuario.TipoSuscripcionId,
                    UsuarioId = user.Id,
                    MedioDePagoId = suscripcionUsuario.MedioDePagoId
                };

                _context.SuscripcionUsuario.Add(subsUser);
                await _context.SaveChangesAsync();

                var susU = _context.SuscripcionUsuario.Include(s => s.TipoSuscripcion).FirstOrDefault(s => s.Id == subsUser.Id);
                var tipoSus = susU.TipoSuscripcion;

                var creador = _context.Creadores.Include(c => c.Usuario).FirstOrDefault(c => c.Id == tipoSus.CreadorId);

                var mensaje = !String.IsNullOrEmpty(tipoSus.MensajeBienvenida) ?
                            tipoSus.MensajeBienvenida :
                            creador.MsjBienvenidaGral;
                var apiKey = Environment.GetEnvironmentVariable("EMAIL__API_KEY");

                if (!String.IsNullOrEmpty(mensaje) && apiKey != null)
                {
                    var client = new SendGridClient(apiKey);
                    var from = new EmailAddress("creadoreu@gmail.com", "CreadoresUY");
                    var subject = $"Bienvenido a la comunidad de {creador.Usuario.UserName}";
                    var to = new EmailAddress(user.Email, user.Username);
                    var plainTextContent = mensaje + (!String.IsNullOrEmpty(creador.VideoYoutube) ? $"\n\n\n<a href='{creador.VideoYoutube}'>Video de bienvenida</a>" : "");
                    var htmlContent = mensaje + (!String.IsNullOrEmpty(creador.VideoYoutube) ? $"\n\n\n<a href='{creador.VideoYoutube}'>Video de bienvenida</a>" : ""); ;
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
                }

                try
                {
                    _estCreadorService.AddSuscripcion(creadorId);
                    _estPlataformaService.AddSuscripcion();

                } catch (Exception ex) {}

                return CreatedAtAction("GetSuscripcionUsuario", new { id = suscripcionUsuario.Id }, _mapper.Map<SuscripcionUsuarioDto>(suscripcionUsuario));
            }
            else
            {
                return BadRequest("Un creador no se puede subscribir a si mismo");
            }

            
        }

        // DELETE: api/SuscripcionUsuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSuscripcionUsuario(int id)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            var suscripcionUsuario = await _context.SuscripcionUsuario.
                Where(m => m.TipoSuscripcionId == id && m.Activo && m.UsuarioId == user.Id).
                FirstOrDefaultAsync();

            if (suscripcionUsuario == null)
            {
                return NotFound();
            }
            var creadorId = await _context.TipoSuscripcion.
               Where(m => m.Id == suscripcionUsuario.TipoSuscripcionId).
               Select(m => m.CreadorId).FirstOrDefaultAsync();

            suscripcionUsuario.Activo = false;
            _context.SuscripcionUsuario.Update(suscripcionUsuario);
            await _context.SaveChangesAsync();

            try
            {
               
                _estCreadorService.SubSuscripcion(creadorId);
                _estPlataformaService.SubSuscripcion();

            }
            catch (Exception ex) { }

            return NoContent();
        }

        [HttpGet("mensajeria/{id}")]
        public async Task<Boolean> puedeEnviarMensajes(string id)
        {
            Boolean res = false; 
            var user = await _identityService.GetUserInfo(HttpContext.User);

            if (user == null || user.Id == id) return false;

            var suscripcionUsuario = _context.SuscripcionUsuario
                                    .Include(s => s.TipoSuscripcion)
                                    .Where(s => s.UsuarioId == user.Id)
                                    .Where(s => s.TipoSuscripcion.CreadorId == id)
                                    .Where(s => s.Activo).FirstOrDefault();
            if (suscripcionUsuario != null)
            {
                int? suscId = suscripcionUsuario.TipoSuscripcionId;
                while (suscId != null)
                {
                    var newSusc = _context.TipoSuscripcion.Find(suscId);
                    if (newSusc.Activo && newSusc.MensajeriaActiva)
                    {
                        return true;
                    }
                    if (newSusc != null && newSusc.IncluyeTipoSuscrId != null)
                    {
                        suscId = newSusc.IncluyeTipoSuscrId;
                    }
                    else
                    {
                        suscId = null;
                    }
                }
            }
            return res;
        }

        private bool SuscripcionUsuarioExists(int id)
        {
            return _context.SuscripcionUsuario.Any(e => e.Id == id);
        }
    }
}
