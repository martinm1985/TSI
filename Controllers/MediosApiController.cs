using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Crud.Models;
using Crud.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Crud.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MediosApiController : ControllerBase
    {

        private readonly ApplicationDbContext _context;

        public MediosApiController(ApplicationDbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// Devuelve todos los medios existentes
        /// </summary>
        /// <returns></returns>
        // GET: api/<ValuesController>
        [HttpGet]
        [Route("api/medios/getAll")]
        public object Get()
        {
            IEnumerable<MediosDePagos> listaMedios = _context.MediosDePagos;

            return JsonConvert.SerializeObject(listaMedios);
        }

        // GET api/<ValuesController>/5
        [HttpGet]
        [Route("api/medios/obtener/{id}")]
        public object Get(int id)
        {

            if (id == 0)
            {
                NotFound();
            }

            //obtener el medio
            var medio = _context.MediosDePagos.Find(id);

            if (medio == null)
            {
                NotFound();
            }

            return JsonConvert.SerializeObject(medio);
        }

        // POST api/<ValuesController>
        [HttpPost]
        [Route("api/medios/actualizar")]
        public void Actualizar(MediosDePagos medio)
        {
            if (ModelState.IsValid)
            {
                _context.MediosDePagos.Update(medio);
                _context.SaveChangesAsync();
                Ok();
            }

        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        [Route("api/medios/crear")]
        public void Crear(MediosDePagos medio)
        {
            if (ModelState.IsValid)
            {
                medio.FechaCreacion = DateTime.Now;
                _context.MediosDePagos.Add(medio);
                _context.SaveChanges();
                Ok();
            }
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        [Route("api/medios/borrar/{id}")]
        public void Borrar(int id)
        {
            if (id == 0)
            {
                NotFound();
            }

            //obtener el medio
            var medio = _context.MediosDePagos.Find(id);

            if (medio == null)
            {
                NotFound();
            }

            _context.MediosDePagos.Remove(medio);
            _context.SaveChanges();
            Ok();
        }

        // Post api/<ValuesController>/StringSearch
        [HttpPost]
        [Route("api/medios/buscar/{searchString}")]
        public object Busqueda(String searchString)
        {
            var medios = from m in _context.MediosDePagos
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                medios = medios.Where(s => s.Detalle.Contains(searchString));
            }

            return JsonConvert.SerializeObject(medios);

        }

        // Post api/<ValuesController>/StringSearch
        [HttpGet]
        [Route("api/medios/cargaDiaria")]
        public List<MediosCreados> cargaDiaria()
        {
            var fechasMedios = from t in _context.MediosDePagos
                               orderby t.FechaCreacion ascending
                               select t.FechaCreacion;


            List<MediosCreados> carga = new List<MediosCreados>();

            SortedSet<DateTime> fechas = new SortedSet<DateTime>();
            //int cantidad;

            foreach (var m in fechasMedios)
            {
                fechas.Add(new DateTime(m.Year,m.Month,m.Day));
            }

            foreach (var f in fechas)
            {
                var sqlCant = from t in _context.MediosDePagos
                              where (t.FechaCreacion >= f && t.FechaCreacion < f.AddDays(1))
                              orderby t.FechaCreacion ascending
                              select t.FechaCreacion;

                MediosCreados nuevo = new MediosCreados(f.ToString("dd/MM/yy"), sqlCant.Count());
                carga.Add(nuevo);
            }

            return carga;

        }

        [HttpPost]
        [Route("api/medios/cargaPorFechas/")]
        public List<MediosCreados> cargaPorFechas(DateTime FechaInicial, DateTime FechaFinal)
        {
            var fechasMedios = from t in _context.MediosDePagos
                               where (t.FechaCreacion >= FechaInicial && t.FechaCreacion <= FechaFinal.AddDays(1))
                               orderby t.FechaCreacion ascending
                               select t.FechaCreacion;


            List<MediosCreados> carga = new List<MediosCreados>();

            SortedSet<DateTime> fechas = new SortedSet<DateTime>();
            //int cantidad;

            foreach (var m in fechasMedios)
            {
                fechas.Add(new DateTime(m.Year, m.Month, m.Day));
            }

            foreach (var f in fechas)
            {
                var sqlCant = from t in _context.MediosDePagos
                              where (t.FechaCreacion >= f && t.FechaCreacion < f.AddDays(1))
                              orderby t.FechaCreacion ascending
                              select t.FechaCreacion;

                MediosCreados nuevo = new MediosCreados(f.ToString("dd/MM/yy"), sqlCant.Count());
                carga.Add(nuevo);
            }

            return carga;

        }
    }
}
