using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Crud.Services;
using Crud.Models;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticaCreadorController : ControllerBase
    {
        private readonly EstadisticaCreadorService _estadisticaCreador;

        public EstadisticaCreadorController(EstadisticaCreadorService estadisticaService)
        {
            _estadisticaCreador = estadisticaService;
        }

        [HttpGet]
        public ActionResult<List<EstadisticaCreador>> Get() =>
            _estadisticaCreador.Get();

        [HttpGet("{id:length(24)}", Name = "GetEstadisticaCreador")]
        public ActionResult<EstadisticaCreador> Get(string id)
        {
            var est = _estadisticaCreador.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            return est;
        }

        [HttpPost]
        public ActionResult<EstadisticaCreador> Create(EstadisticaCreador est)
        {
            _estadisticaCreador.Create(est);

            return CreatedAtRoute("GetEstadisticaCreador", new { id = est.Id.ToString() }, est);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, EstadisticaCreador estIn)
        {
            var est = _estadisticaCreador.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaCreador.Update(id, estIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var est = _estadisticaCreador.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaCreador.Remove(est.Id);

            return NoContent();
        }

    }
}
