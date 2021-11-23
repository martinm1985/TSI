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
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ImagenController(ApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }

        // GET: api/Imagen
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Imagen>>> GetImagen()
        {
            return await _context.Imagen.ToListAsync();
        }

        // GET: api/Imagen/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Object>> GetImagen(int id)
        {
            var imagen = _context.Imagen
                         .Include(c => c.Categoria)
                         .Include(c => c.Creador)
                         .Where(c => (c.Id == id))
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
                             DuracionVideo = 0,
                             Url = "",
                             FechaInicio = "",
                             FechaFin = "",
                         }).FirstOrDefault();

            imagen.ArchivoContenido = getImageBlob(imagen.Archivo);

            if (imagen == null)
            {
                return NotFound();
            }

            return imagen;
        }

        // PUT: api/Imagen/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutImagen(int id, Imagen imagen)
        {
            if (id != imagen.Id)
            {
                return BadRequest();
            }

            _context.Entry(imagen).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ImagenExists(id))
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

        // POST: api/Imagen
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Imagen>> PostImagen(ContenidoDto.ContenidoRegistro contenido)
        {

            var user = await _identityService.GetUserInfo(HttpContext.User);
            var imagen = new Imagen
            {
                CreadorId = user.Id,
                Titulo = contenido.Titulo,
                Descripcion = contenido.Descripcion,
                FechaCreacion = DateTime.Now.Date,
                Bloqueado = false,
                DerechoAutor = contenido.DerechoAutor,
                Archivo = contenido.Archivo,
                Calidad = contenido.Calidad,
                CategoriaId = contenido.CategoriaId,
                TipoSuscripcionId = contenido.TipoSuscripcionId,
            };

            _context.Imagen.Add(imagen);
            await _context.SaveChangesAsync();
            return Ok(); 
        }

        // DELETE: api/Imagen/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagen(int id)
        {
            var imagen = await _context.Imagen.FindAsync(id);
            if (imagen == null)
            {
                return NotFound();
            }

            _context.Imagen.Remove(imagen);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ImagenExists(int id)
        {
            return _context.Imagen.Any(e => e.Id == id);
        }

        private string getImageBlob(string filename)
        {
            byte[] contenido = FTPElastic.DownloadFileFTP(filename);
            Regex regex = new Regex(".*\\.");
            string type = regex.Replace(filename, string.Empty);
            return "data:image/" + type + ";base64," + Convert.ToBase64String(contenido);
        }
    }
}
