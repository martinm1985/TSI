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
        private readonly EstadisticaContenidoService _estContenidoService;

        public ContenidoController(ApplicationDbContext context, IIdentityService identityService, EstadisticaContenidoService estContenidoService)
        {
            _identityService = identityService;
            _context = context;
            _estContenidoService = estContenidoService;
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

        // GET: api/Contenido/search
        [HttpGet("searchMobile")]
        public async Task<ActionResult<Object>> GetContenidoSearchMobile(string? search, int page, int pagesize)
        {

            IEnumerable<Object> contenidos;
            int total = 0;
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
                               });
                Console.WriteLine('1');
                total = contenidos.Count();
                Console.WriteLine('2');
                contenidos = contenidos
                               .Skip((page - 1) * pagesize)
                               .Take(pagesize);
                Console.WriteLine('3');

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
                               });
                total = contenidos.Count();
                contenidos = contenidos
                               .Skip((page - 1) * pagesize)
                               .Take(pagesize);
            }

            return new {
                total = total,
                values = contenidos.ToList()
            };
        }



        // GET: api/Contenido/search
        [HttpGet("creador")]
        public async Task<ActionResult<IEnumerable<Object>>> GetAllContenidoCreador(string id, int page, int pageSize)
        {
            if (id == null || id == "") return BadRequest();

            var contenidos = _context.Contenido
                               .Where(c => (!c.Bloqueado))
                               .Include(c => c.Categoria)
                               .Include(c => c.Creador)
                               .Where(c => c.CreadorId == id)
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
                               .Skip((page - 1) * pageSize)
                               .Take(pageSize);

            return await contenidos.AsQueryable().ToListAsync();
        }

        // GET: api/Contenido/search
        [HttpGet("creadorMobile")]
        public async Task<ActionResult<Object>> GetAllContenidoCreadorMobile(string id, int page, int pageSize)
        {
            if (id == null || id == "") return BadRequest();

            Console.WriteLine(id);
            Console.WriteLine(pageSize);
            Console.WriteLine(page);
            var contenidos = _context.Contenido
                               .Where(c => (!c.Bloqueado))
                               .Include(c => c.Categoria)
                               .Include(c => c.Creador)
                               .Where(c => c.CreadorId == id)
                               .OrderByDescending(c => c.FechaCreacion)
                               .Select(item => new
                               {
                                   id = item.Id,
                                   username = item.Creador.Usuario.UserName,
                                   titulo = item.Titulo,
                                   descripcion = item.Descripcion,
                                   fechaCreacion = item.FechaCreacion.ToShortDateString(),
                                   categoriaId = item.CategoriaId,
                                   categoriaNombre = item.Categoria.Nombre
                               });

            var total = contenidos.Count();

            contenidos = contenidos.Skip((page - 1) * pageSize).Take(pageSize);
            Console.WriteLine("Ejecuta=??????");
            return new {
                total = total,
                values = await contenidos.ToListAsync()
            };
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
                          where (tipoSusc == null || tipoSusc.Activo)

                          orderby cont.FechaCreacion

                          select new ContenidoDto.GetAllContenido
                          {
                              Id = cont.Id,
                              CreadorId = cont.CreadorId,
                              Username = cont.Creador.Usuario.UserName,
                              Titulo = cont.Titulo,
                              Descripcion = cont.Descripcion,
                              FechaCreacion = cont.FechaCreacion.ToShortDateString(),
                              CategoriaId = cont.CategoriaId,
                              CategoriaNombre = cont.Categoria.Nombre,
                              Texto = texto.Html,
                              Largo = texto.Largo == null ? (int?)null : texto.Largo,
                              Archivo = cont.Archivo,
                              Calidad = cont.Calidad,
                              Duracion = audio.Duracion == null ? (decimal?)null : audio.Duracion,
                              DuracionVideo = video.Duracion == null ? (decimal?)null : video.Duracion,
                              Url = link.Url,
                              FechaInicio = liveStream.FechaInicio.ToShortDateString(),
                              FechaFin = liveStream.FechaFin.ToShortDateString(),
                              SuscripcionId = tipoSusc.Id,
                          });

            if (user != null && user.isAdministrador)
            {
                result = result.Skip((page - 1) * pageSize).Take(pageSize);
                return Ok(result);
            }

            var suscripcionUsuario = _context.SuscripcionUsuario
                                    .Where(s => s.UsuarioId == user.Id)
                                    .Where(s => s.Activo)
                                    .FirstOrDefault();


            Dictionary<int, int> suscripcionesDict = new Dictionary<int, int>();

            if (suscripcionUsuario != null)
            {
                int? suscId = suscripcionUsuario.TipoSuscripcionId;
                while (suscId != null)
                {
                    suscripcionesDict.Add((int)suscId, (int)suscId);
                    var addSusc = _context.TipoSuscripcion.Find(suscId);
                    if(addSusc != null && addSusc.IncluyeTipoSuscrId != null)
                    {
                        suscId = addSusc.IncluyeTipoSuscrId;
                    } else
                    {
                        suscId = null;
                    }
                }
            }
            var contenidoResult = new List<ContenidoDto.GetAllContenido>();

            foreach (var item in result)
            {
                if (item.SuscripcionId == null || item.CreadorId == user.Id)
                {
                    // No tiene suscripcion (es gratis) o soy el creador del contenido -> puedo ver el contenido
                    contenidoResult.Add(item);
                }
                else
                {
                    int value = 0;
                    if (suscripcionesDict.TryGetValue((int)item.SuscripcionId, out value))
                    {
                        // Esta entre las suscripciones incluidas al usuario -> puedo ver el contenido
                        contenidoResult.Add(item);
                    }
                }
            }
            contenidoResult = contenidoResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return Ok(contenidoResult);
        }

        // GET: api/Contenido/allMobile
        [HttpGet("allMobile")]
        public async Task<ActionResult<IEnumerable<Object>>> GetAllContenidoMobile(int page, int pageSize)
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
                          where (tipoSusc == null || tipoSusc.Activo)

                          orderby cont.FechaCreacion

                          select new ContenidoDto.GetAllContenido
                          {
                              Id = cont.Id,
                              CreadorId = cont.CreadorId,
                              Username = cont.Creador.Usuario.UserName,
                              Titulo = cont.Titulo,
                              Descripcion = cont.Descripcion,
                              FechaCreacion = cont.FechaCreacion.ToShortDateString(),
                              CategoriaId = cont.CategoriaId,
                              CategoriaNombre = cont.Categoria.Nombre,
                              Texto = texto.Html,
                              Largo = texto.Largo == null ? (int?)null : texto.Largo,
                              Archivo = cont.Archivo,
                              Calidad = cont.Calidad,
                              Duracion = audio.Duracion == null ? (decimal?)null : audio.Duracion,
                              DuracionVideo = video.Duracion == null ? (decimal?)null : video.Duracion,
                              Url = link.Url,
                              FechaInicio = liveStream.FechaInicio.ToShortDateString(),
                              FechaFin = liveStream.FechaFin.ToShortDateString(),
                              SuscripcionId = tipoSusc.Id,
                          });
                          
            var total = result.Count();
            if (user != null && user.isAdministrador)
            {
                result = result.Skip((page - 1) * pageSize).Take(pageSize);
                return Ok(new ContenidoDto.GetAllContenidoMobile {
                total = total,
                values = result.ToList()
            });
            }

            var suscripcionUsuario = _context.SuscripcionUsuario
                                    .Where(s => s.UsuarioId == user.Id)
                                    .Where(s => s.Activo)
                                    .FirstOrDefault();


            Dictionary<int, int> suscripcionesDict = new Dictionary<int, int>();

            if (suscripcionUsuario != null)
            {
                int? suscId = suscripcionUsuario.TipoSuscripcionId;
                while (suscId != null)
                {
                    suscripcionesDict.Add((int)suscId, (int)suscId);
                    var addSusc = _context.TipoSuscripcion.Find(suscId);
                    if(addSusc != null && addSusc.IncluyeTipoSuscrId != null)
                    {
                        suscId = addSusc.IncluyeTipoSuscrId;
                    } else
                    {
                        suscId = null;
                    }
                }
            }
            var contenidoResult = new List<ContenidoDto.GetAllContenido>();

            foreach (var item in result)
            {
                if (item.SuscripcionId == null || item.CreadorId == user.Id)
                {
                    // No tiene suscripcion (es gratis) o soy el creador del contenido -> puedo ver el contenido
                    contenidoResult.Add(item);
                }
                else
                {
                    int value = 0;
                    if (suscripcionesDict.TryGetValue((int)item.SuscripcionId, out value))
                    {
                        // Esta entre las suscripciones incluidas al usuario -> puedo ver el contenido
                        contenidoResult.Add(item);
                    }
                }
            }
            
            total = contenidoResult.Count();
            contenidoResult = contenidoResult.Skip((page - 1) * pageSize).Take(pageSize).ToList();


            return Ok(new ContenidoDto.GetAllContenidoMobile {
                total = total,
                values = contenidoResult
            });
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

            try
            {
                _estContenidoService.AddVisualizacion(id);
            }
            catch (Exception){}

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
               Descripcion = "Contenido bloqueado.",
            };

            var contenido = await _context.Contenido.FindAsync(id);
            if (contenido == null)
            {
                resultado.Descripcion = "Contenido no encontrado";
                return resultado;
            }

            if (!(user != null && (user.Id == contenido.CreadorId || user.isAdministrador)))
            {
                // Chequeo si el usuario puede ver el contenido
                var suscripcionContenido = _context.TipoSuscripcion
                                     .Where(s => s.Id == contenido.TipoSuscripcionId).FirstOrDefault();

                if (suscripcionContenido != null)
                {
                    if (user == null) return resultado;

                    // Si no es público chequeo que el usuario logueado tenga una suscripcion con el creador
                    var suscripcionUsuario = _context.SuscripcionUsuario
                                               .Include(s => s.TipoSuscripcion)
                                                .Where(s => s.UsuarioId == user.Id)
                                                .Where(s => s.TipoSuscripcion.CreadorId == contenido.CreadorId)
                                                .Where(s => s.Activo).FirstOrDefault();

                    if (suscripcionUsuario == null) return resultado;

                    if (suscripcionUsuario.TipoSuscripcionId != contenido.TipoSuscripcionId)
                    {
                        // Esta incluido ?
                        int? suscId = suscripcionUsuario.TipoSuscripcionId;
                        while (suscId != null && suscId != contenido.TipoSuscripcionId)
                        {
                            var addSusc = _context.TipoSuscripcion.Find(suscId);
                            if (addSusc != null && addSusc.IncluyeTipoSuscrId != null)
                            {
                                suscId = addSusc.IncluyeTipoSuscrId;
                            }
                            else
                            {
                                suscId = null;
                                return resultado;
                            }
                        }
                    }
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

            try
            {
                _estContenidoService.AddVisualizacion(id);
            }
            catch (Exception){}

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

        [HttpPost("blockContent/{id}")]
        public async Task<ActionResult> BlockContent(int id)
        {
            var user = await _identityService.GetUserInfo(HttpContext.User);
            if (user == null || !user.isAdministrador )
            {
                return Unauthorized();
            }
            var contenido = _context.Contenido.Find(id);
            if (contenido == null)
            {
                return BadRequest();
            }
            contenido.Bloqueado = true;
            _context.Entry(contenido).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }

            return Ok();
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
