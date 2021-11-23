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
    public class ContenidoController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public ContenidoController(ApplicationDbContext context, IIdentityService identityService)
        {
            _identityService = identityService;
            _context = context;
        }

        // GET: api/Contenido
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contenido>>> GetContenido()
        {
            return Ok(await _context.Contenido.ToListAsync());
        }

        // GET: api/Contenido/search
        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<Object>>> GetContenidoSearch(string? search)
        {

            IEnumerable<Object> contenidos;

            if (!String.IsNullOrEmpty(search))
            {
                contenidos = _context.Contenido
                               .Where(c => (!c.Bloqueado) && (c.Titulo.Contains(search) || c.Descripcion.Contains(search)))
                               .Include(c => c.Categoria)
                               .Include(c => c.Creador)
                               .Select(item => new
                               {
                                   id = item.Id,
                                   username = item.Creador.Usuario.UserName,
                                   titulo = item.Titulo,
                                   descripcion = item.Descripcion,
                                   fechaCreacion = item.FechaCreacion.ToShortDateString(),
                                   categoriaId = item.CategoriaId,
                                   categoriaNombre = item.Categoria.Nombre
                               })
                               .Take(5);
            }
            else
            {
                contenidos = _context.Contenido
                               .Where(c => (!c.Bloqueado))
                               .Include(c => c.Categoria)
                               .Include(c => c.Creador)
                               .Select(item => new
                               {
                                   id = item.Id,
                                   username = item.Creador.Usuario.UserName,
                                   titulo = item.Titulo,
                                   descripcion = item.Descripcion,
                                   fechaCreacion = item.FechaCreacion.ToShortDateString(),
                                   categoriaId = item.CategoriaId,
                                   categoriaNombre = item.Categoria.Nombre
                               }).Take(5);
            }

            return await contenidos.AsQueryable().ToListAsync();
        }


        // GET: api/Contenido/all
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<Object>>> GetAllContenido(int page, int pageSize)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            var result = (from cont in _context.Contenido
                          join texto in _context.Texto on cont.Id equals texto.Id into DTexto
                          from texto in DTexto.DefaultIfEmpty()
                          join imagen in _context.Imagen on cont.Id equals imagen.Id into DImagen
                          from imagen in DImagen.DefaultIfEmpty()
                          join link in _context.Link on cont.Id equals link.Id into DLink
                          from link in DLink.DefaultIfEmpty()
                          join liveStream in _context.LiveStream on cont.Id equals liveStream.Id into DliveStream
                          from liveStream in DliveStream.DefaultIfEmpty()
                          join audio in _context.Audio on cont.Id equals audio.Id into DAudio
                          from audio in DAudio.DefaultIfEmpty()
                          join video in _context.Video on cont.Id equals video.Id into DVideo
                          from video in DVideo.DefaultIfEmpty()

                          join tipoSusc in _context.TipoSuscripcion on cont.TipoSuscripcionId equals tipoSusc.Id into DTipoSusc
                          from tipoSusc in DTipoSusc.DefaultIfEmpty()

                          join suscUsuario in _context.SuscripcionUsuario on cont.TipoSuscripcionId equals suscUsuario.TipoSuscripcionId into DsuscUsuario
                          from suscUsuario in DsuscUsuario.DefaultIfEmpty()

                          where cont.Bloqueado == false
                          where (tipoSusc.Precio == 0 || (tipoSusc.Precio != 0 && 
                                suscUsuario.UsuarioId != null && suscUsuario.UsuarioId == user.Id
                                && suscUsuario.FechaFin == null && suscUsuario.Activo)) 
                               // Es público o estoy suscripto y esta activa la suscripcion 
                               // FALTA tomar en cuenta las suscripciones que estan incluidas !
                          where tipoSusc.Activo

                          orderby cont.FechaCreacion

                          select new
                          {
                              id = cont.Id,
                              username = cont.Creador.Usuario.UserName,
                              titulo = cont.Titulo,
                              descripcion = cont.Descripcion,
                              fechaCreacion = cont.FechaCreacion.ToShortDateString(),
                              categoriaId = cont.CategoriaId,
                              categoriaNombre = cont.Categoria.Nombre,
                              texto = texto.Html,
                              largo = texto.Largo == null ? (int?)null : texto.Largo,
                              archivo = cont.Archivo,
                              calidad = cont.Calidad,
                              duracion = audio.Duracion == null ? (decimal?)null : audio.Duracion,
                              duracionVideo = video.Duracion == null ? (decimal?)null : video.Duracion,
                              url = link.Url,
                              fechaInicio = liveStream.FechaInicio.ToShortDateString(),
                              fechaFin = liveStream.FechaFin.ToShortDateString(),

                          })
                          .Skip((page - 1) * pageSize)
                          .Take(pageSize);

            return Ok(result);
        }


        // GET: api/Contenido/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Contenido>> GetContenido(int id)
        {
            var contenido = await _context.Contenido.FindAsync(id);

            if (contenido == null)
            {
                return NotFound();
            }

            return contenido;
        }

        // GET: api/Contenido/5
        [HttpGet("cast/{id}")]
        public async Task<ActionResult<ContenidoDto.ContenidoCast>> GetContenidoCast(int id)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);

            var resultado = new ContenidoDto.ContenidoCast
            {
               Cast = -1,
               Descripcion = "",
            };

            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null)
            {
                resultado.Descripcion = "Contenido no encontrado";
                return resultado;
            }
            // Chequeo si el usuario puede ver el contenido 
            var suscripcionContenido = _context.TipoSuscripcion
                                 .Where(s => s.Id == contenido.TipoSuscripcionId).FirstOrDefault();
            if (suscripcionContenido.Precio != 0)
            {
                // Si no es público chequeo que este suscripto  // hacerlo recursivo 
                var susUsuario = _context.SuscripcionUsuario
                                    .Where(s => (s.TipoSuscripcionId == suscripcionContenido.Id
                                        || s.TipoSuscripcionId == suscripcionContenido.IncluyeTipoSuscrId)).FirstOrDefault();
                if (susUsuario == null)
                {
                    resultado.Descripcion = "Contenido bloqueado";
                    return resultado;
                }
            }

            // Chequeo el tipo 
            resultado.Cast = 1;
            if (contenido is Texto)
                resultado.Descripcion = "Texto";
            else if (contenido is Link)
                resultado.Descripcion = "Link";
            else if (contenido is LiveStream)
                resultado.Descripcion = "LiveStream";
            else if (contenido is Audio)
                resultado.Descripcion = "Audio";
            else if (contenido is Video)
                resultado.Descripcion = "Video";
            else if (contenido is Imagen)
                resultado.Descripcion = "Imagen";

            return resultado;
        }

        // PUT: api/Contenido/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContenido(int id, Contenido contenido)
        {
            if (id != contenido.Id)
            {
                return BadRequest();
            }

            _context.Entry(contenido).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContenidoExists(id))
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

        // POST: api/Contenido
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Contenido>> PostContenido(Contenido contenido)
        {
            _context.Contenido.Add(contenido);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContenido", new { id = contenido.Id }, contenido);
        }

        // DELETE: api/Contenido/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContenido(int id)
        {
            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null)
            {
                return NotFound();
            }

            _context.Contenido.Remove(contenido);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ContenidoExists(int id)
        {
            return _context.Contenido.Any(e => e.Id == id);
        }


    }
}
