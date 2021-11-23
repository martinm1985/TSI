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
    public class VideoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public VideoController(ApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }

        // GET: api/Video
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Video>>> GetVideo()
        {
            return await _context.Video.ToListAsync();
        }

        // GET: api/Video/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetVideo(int id)
        {
            var video = _context.Video
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
                             Duracion = 0,
                             DuracionVideo = item.Duracion,
                             Url = "",
                             FechaInicio = "",
                             FechaFin = "",
                         }).FirstOrDefault();

            video.ArchivoContenido = getVideoBlob(video.Archivo);

            if (video == null)
            {
                return NotFound();
            }

            return video;
        }

        // PUT: api/Video/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVideo(int id, Video video)
        {
            if (id != video.Id)
            {
                return BadRequest();
            }

            _context.Entry(video).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!VideoExists(id))
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

        // POST: api/Video
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Video>> PostVideo(ContenidoDto.ContenidoRegistro contenido)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            var video = new Video
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
            _context.Video.Add(video);
            await _context.SaveChangesAsync();

            return Ok(); // CreatedAtAction("GetVideo", new { id = video.Id }, video);
        }

        // DELETE: api/Video/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVideo(int id)
        {
            var video = await _context.Video.FindAsync(id);
            if (video == null)
            {
                return NotFound();
            }

            _context.Video.Remove(video);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool VideoExists(int id)
        {
            return _context.Video.Any(e => e.Id == id);
        }
        private string getVideoBlob(string filename)
        {
            byte[] contenido = FTPElastic.DownloadFileFTP(filename);
            Regex regex = new Regex(".*\\.");
            string type = regex.Replace(filename, string.Empty);
            return "data:video/" + type + ";base64," + Convert.ToBase64String(contenido);
        }
    }
}
