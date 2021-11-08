using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Crud.Services;
using Crud.Models;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticaPlataformaController : ControllerBase
    {
        private readonly EstadisticaPlataformaService _estadisticaPlataforma;

        public EstadisticaPlataformaController(EstadisticaPlataformaService estadisticaService)
        {
            _estadisticaPlataforma = estadisticaService;
        }

        [HttpGet]
        public ActionResult<List<EstadisticaPlataforma>> Get() =>
            _estadisticaPlataforma.Get();

        [HttpGet("{id:length(24)}", Name = "GetEstadisticaPlataforma")]
        public ActionResult<EstadisticaPlataforma> Get(string id)
        {
            var est = _estadisticaPlataforma.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            return est;
        }

        [HttpPost]
        public ActionResult<EstadisticaPlataforma> Create(EstadisticaPlataforma est)
        {
            _estadisticaPlataforma.Create(est);

            return CreatedAtRoute("GetEstadisticaPlataforma", new { id = est.Id.ToString() }, est);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, EstadisticaPlataforma estIn)
        {
            var est = _estadisticaPlataforma.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaPlataforma.Update(id, estIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var est = _estadisticaPlataforma.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaPlataforma.Remove(est.Id);

            return NoContent();
        }
    
}
}
