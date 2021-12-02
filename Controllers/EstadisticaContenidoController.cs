using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Crud.Services;
using Crud.Models;
using System;

namespace Crud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EstadisticaContenidoController : ControllerBase
    {
        private readonly EstadisticaContenidoService _estadisticaContenido;

        public EstadisticaContenidoController(EstadisticaContenidoService estadisticaService)
        {
            _estadisticaContenido = estadisticaService;
        }

        [HttpGet]
        public ActionResult<List<EstadisticaContenido>> Get() =>
            _estadisticaContenido.Get();

        [HttpGet("{id:length(24)}", Name = "GetEstadisticaContenido")]
        public ActionResult<List<EstadisticaContenido>> Get(int id)
        {
            var est = _estadisticaContenido.GetByIdContenido(id);

            if (est == null)
            {
                return NotFound();
            }

            return est;
        }

        [HttpPost]
        public ActionResult<EstadisticaContenido> Create(EstadisticaContenido est)
        {
            _estadisticaContenido.Create(est);

            return CreatedAtRoute("GetEstadisticaContenido", new { id = est.Id.ToString() }, est);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, EstadisticaContenido estIn)
        {
            var est = _estadisticaContenido.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaContenido.Update(id, estIn);

            return NoContent();
        }

        [HttpPut("{idContenido}")]
        public IActionResult AddVisualizacion(int idContenido)
        {
            var est = _estadisticaContenido.GetByIdContenidoAndDate(idContenido, DateTime.Today);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaContenido.AddVisualizacion(idContenido);

            return NoContent();
        }


        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var est = _estadisticaContenido.Get(id);

            if (est == null)
            {
                return NotFound();
            }

            _estadisticaContenido.Remove(est.Id);

            return NoContent();
        }

    }
}
