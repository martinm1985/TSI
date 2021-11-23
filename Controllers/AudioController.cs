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
using System.Text.RegularExpressions;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AudioController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public AudioController(ApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
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
        public async Task<ActionResult<Object>> GetAudio(int id)
        {
            var audio = _context.Audio
                         .Include(c => c.Categoria)
                         .Include(c => c.Creador)
                         .Where(c => c.Id == id)
                         .Select(item => new ContenidoDto.GetContenidoData
                         {
                             Id = item.Id,
                             Username = item.Creador.Usuario.UserName,
                             Titulo = item.Titulo,
                             Descripcion = item.Descripcion,
                             FechaCreacion = item.FechaCreacion.ToShortDateString(),
                             CategoriaId = item.CategoriaId,
                             CategoriaNombre = item.Categoria.Nombre,
                             Texto = "",
                             Largo = 0,
                             Archivo = item.Archivo,
                             ArchivoContenido = "",
                             Calidad = item.Calidad,
                             Duracion = item.Duracion,
                             DuracionVideo = 0,
                             Url = "",
                             FechaInicio = "", 
                             FechaFin = "",
                         }).FirstOrDefault();

            audio.ArchivoContenido = getAudioBlob(audio.Archivo);

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
        public async Task<ActionResult<Audio>> PostAudio(ContenidoDto.ContenidoRegistro contenido)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            var audio = new Audio
            {
                CreadorId = user.Id,
                Titulo = contenido.Titulo,
                Descripcion = contenido.Descripcion,
                FechaCreacion = DateTime.Now.Date,
                Bloqueado = false,
                DerechoAutor = contenido.DerechoAutor,
                Archivo = contenido.Archivo,
                Duracion = Convert.ToDecimal(contenido.Duracion),
                Calidad = contenido.Calidad,
                CategoriaId = contenido.CategoriaId,
                TipoSuscripcionId = contenido.TipoSuscripcionId,
            };

            _context.Audio.Add(audio);
            await _context.SaveChangesAsync();

            return Ok(); //  CreatedAtAction("GetAudio", new { id = audio.Id }, audio);
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

        private string getAudioBlob(string filename)
        {
            byte[] contenido = FTPElastic.DownloadFileFTP(filename);
            Regex regex = new Regex(".*\\.");
            string type = regex.Replace(filename, string.Empty);
            return "data:audio/" + type + ";base64," + Convert.ToBase64String(contenido);
        }
    }
}
