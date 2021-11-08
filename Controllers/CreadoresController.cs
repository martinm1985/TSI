using Crud.Data;
using Crud.DTOs;
using Crud.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreadoresController : ControllerBase
    {

        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public CreadoresController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }

        // GET: api/<CreadoresController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserData>>> GetCreadores()
        {
            var listCreatorsUsers = _mapper.Map<List<UserData>>(
                await _context.Creadores.Include(s => s.Usuario.Creador).Select(c => c.Usuario).ToListAsync()
            );
            return Ok(listCreatorsUsers);
        }

        // GET api/<CreadoresController>/5
        [HttpGet("{id}")]
        public ActionResult<CreadorData> GetCreador(string id)
        {
            var creador = _context.Creadores.Find(id);

            if (creador == null)
            {
                return NotFound();
            }

            return creador.GetCreadorData();
        }


        // POST api/<CreadoresController>
        [HttpPost("register")]
        public ActionResult<CreadorData> PostCreador(CreadorRegister creadorRegister)
        {
            var creador = new Creador
            {
                Descripcion = creadorRegister.Descripcion,
                Imagen = creadorRegister.Imagen,
                ImagePortada = creadorRegister.ImagePortada,
                Biografia = creadorRegister.Biografia,
                VideoYoutube = creadorRegister.VideoYoutube,
                MsjBienvenidaGral = creadorRegister.MsjBienvenidaGral,
                Id = creadorRegister.UserId,
                Categoria1 = _context.Categoria.Find(creadorRegister.Categoria1Id),
                Categoria2 = _context.Categoria.Find(creadorRegister.Categoria2Id),
                UserId = creadorRegister.UserId
            };

            _context.Creadores.Add(creador);
            _context.SaveChanges();

            return CreatedAtAction("GetCreador", new { id = creador.Id }, creador);
        }

        // PUT api/<CreadoresController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CreadoresController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
