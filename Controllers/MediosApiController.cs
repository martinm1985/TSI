using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Crud.Models;
using Crud.Data;
using Crud.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AutoMapper;
using Crud.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Crud.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class MediosApiController : ControllerBase
    {
        private readonly UserManager<User> _userManager;

        private readonly ApplicationDbContext _context;

        private readonly IMapper _mapper;

        public MediosApiController(ApplicationDbContext context, UserManager<User> userManager, IMapper mapper)
        {
            _context = context;
            _userManager = userManager;
            _mapper = mapper;
        }


        /// <summary>
        /// Devuelve todos los medios existentes
        /// </summary>
        /// <returns></returns>
        // GET: api/<ValuesController>
        [HttpGet]
        [Route("api/medios/getAll")]
        public object GetMedios()
        {
            IEnumerable<MediosDePagos> listaMedios = _context.MediosDePagos;

            return JsonConvert.SerializeObject(listaMedios);
        }

        [HttpGet]
        [Route("api/medios/getUserAll/{id}")]
        public object GetMediosUser(string id)
        {
            var medios = from m in _context.MediosDePagos
                         join e in _context.EntidadesFinancieras on m.IdEntidadFinanciera equals e.Id
                         where m.UserId == id && !m.Borrado &&  !e.Borrado
                         select m;

            return JsonConvert.SerializeObject(medios);
        }

        [HttpGet]
        [Route("api/medios/getEntidadesFinancieras")]
        public ActionResult<IEnumerable<EntidadFinanciera>> GetEntidadesFinancieras()
        {
            return Ok(_context.EntidadesFinancieras.Where(m => !m.Borrado).Select(m => m).ToList());
        }

        [HttpGet]
        [Route("api/entidades_financieras/getEntidadesFinancierasCredito")]
        public object GetEntidadesFinancierasTarjetasCredito()
        {
            var entidades = from m in _context.EntidadesFinancieras
                         where m.TarjetaCredito == true && !m.Borrado
                         select m;

            return JsonConvert.SerializeObject(entidades.ToList());
        }

        [HttpGet]
        [Route("api/entidades_financieras/getEntidadesFinancierasDebito")]
        public object GetEntidadesFinancierasTarjetasDebito()
        {
            var entidades = from m in _context.EntidadesFinancieras
                            where m.TarjetaDebito == true && !m.Borrado
                            select m;

            return JsonConvert.SerializeObject(entidades);
        }

        [HttpGet]
        [Route("api/entidades_financieras/getEntidadesFinancierasTarjeta")]
        public object GetEntidadesFinancierasTarjetasCuenta()
        {
            var entidades = from m in _context.EntidadesFinancieras
                            where m.Cuenta == true && !m.Borrado
                            select m;

            return JsonConvert.SerializeObject(entidades);
        }

        [HttpGet]
        [Route("api/medios/getTarjetas")]
        public async Task<ActionResult<IEnumerable<TarjetaResponse>>> GetTarjetasAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            //obtener el medio
            var medios = _context.Tarjetas.Where(m => m.UserId == user.Id && !m.Borrado)
                .Include(m => m.EntidadFinanciera);
                       
                         

            if (medios == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<List<TarjetaResponse>>(await medios.ToListAsync()));
        }

        [HttpGet]
        [Route("api/medios/getCuentasUser")]
        public async Task<ActionResult<IEnumerable<CuentaResponse>>> GetCuentasUser()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            //obtener el medio
            var medios = _context.Cuentas.Where(m => m.UserId == user.Id && !m.Borrado)
                .Include(m => m.EntidadFinanciera);

            if (medios == null)
            {
                NotFound();
            }

            return Ok(_mapper.Map<List<CuentaResponse>>(await medios.ToListAsync()));
        }

        [HttpGet]
        [Route("api/medios/getPayPalsUser")]
        public async Task<ActionResult<IEnumerable<PayPal>>> GetPayPalUser()
        {

            var user = await _userManager.GetUserAsync(HttpContext.User);

            //obtener el medio
            var medios = from m in _context.Paypals
                         where m.UserId == user.Id && !m.Borrado
                         select m;

            if (medios == null)
            {
                NotFound();
            }

            return Ok(medios.ToList());
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
            var medio = _context.MediosDePagos.
                Where(m=> m.Id == id && !m.Borrado).
                Select(m => m).First();

            if (medio == null)
            {
                NotFound();
            }

            return JsonConvert.SerializeObject(medio);
        }

        [HttpGet]
        [Route("api/entidad_financiera/obtener/{id}")]
        public object GetEntidadFinanciera(int id)
        {

            if (id == 0)
            {
                NotFound();
            }

            //obtener el medio
            var entidad = _context.EntidadesFinancieras.
                Where(m => m.Id == id && !m.Borrado).
                Select(m => m).First();

            if (entidad == null)
            {
                NotFound();
            }

            return JsonConvert.SerializeObject(entidad);
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

        [HttpPost]
        [Route("api/entidad_financiera/actualizar")]
        public async Task<IActionResult> ActualizarEntidadFinancieraAsync(EntidadFinanciera entidad)
        {
            var entidadAux = from m in _context.EntidadesFinancieras
                             where m.Nombre == entidad.Nombre &&
                             m.Id != entidad.Id
                             select m;

            if (entidadAux.Count() > 0)
            {
                return BadRequest("Ya existe una entidad financiera con ese nombre");
                
            }

            var entidadFinanciera = _context.EntidadesFinancieras.Find(entidad.Id);

            if (ModelState.IsValid)
            {

                entidadFinanciera.Nombre = entidad.Nombre;
                entidadFinanciera.Direccion = entidad.Direccion;
                entidadFinanciera.Telefono = entidad.Telefono;
                entidadFinanciera.TarjetaCredito = entidad.TarjetaCredito;
                entidadFinanciera.TarjetaDebito = entidad.TarjetaDebito;
                entidadFinanciera.Cuenta = entidad.Cuenta;

                try
                {
                    _context.EntidadesFinancieras.Update(entidadFinanciera);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error al guardar");
                }


            }

            return BadRequest("No coinciden los parámetros de entrada");
        }

        // PUT api/<ValuesController>/5
        [HttpPut]
        [Route("api/medios/crear")]
        public void Crear(MediosDePagos medio)
        {
            if (ModelState.IsValid)
            {
                medio.FechaCreacion = DateTime.Now;
                medio.Borrado = false;
                medio.Activo = true;
                _context.MediosDePagos.Add(medio);
                _context.SaveChanges();
                Ok();
            }
        }

        // PUT api/<ValuesController>/5
        [HttpPost]
        [Route("api/entidad_financiera/crear")]
        public IActionResult CrearEntidadFinanciera(EntidadFinancieraRequest entidadReq)
        {
            var entidadAux = from m in _context.EntidadesFinancieras
                             where m.Nombre == entidadReq.Nombre
                             select m;

            if (entidadAux.Count() > 0)
            {
                return BadRequest("Ya existe una entidad financiera con ese nombre");
                
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var entidad = new EntidadFinanciera
                    {
                        Nombre = entidadReq.Nombre,
                        Direccion = entidadReq.Direccion,
                        Telefono = entidadReq.Telefono,
                        TarjetaCredito = entidadReq.TarjetaCredito,
                        TarjetaDebito = entidadReq.TarjetaDebito,
                        Cuenta = entidadReq.Cuenta,
                        Borrado = false
                    };

                    _context.EntidadesFinancieras.Add(entidad);
                    _context.SaveChanges();
                    return Ok();
                }

                return BadRequest("No coinciden los parámetros de entrada");
            }

        }

        // PUT api/<ValuesController>/5
        [HttpPost]
        [Route("api/medios/crear/Tarjeta")]
        public IActionResult CrearTarjetaUsuario(TarjetaRequest tarjetaReq)
        {

            var tarjetaAux = from m in _context.Tarjetas
                             where m.NumeroTarjeta == tarjetaReq.NumeroTarjeta &&
                             m.IdEntidadFinanciera == tarjetaReq.IdEntidadFinanciera &&
                             m.UserId == tarjetaReq.IdUser
                             select m;

            if (tarjetaAux.Count() > 0)
            {
                return BadRequest("Ya existe una tarjeta con ese número para esa entidad financiera");

            }


            if (ModelState.IsValid)
            {
                var tarjeta = new Tarjeta
                {
                    UserId = tarjetaReq.IdUser,
                    Detalle = tarjetaReq.Detalle,
                    IdEntidadFinanciera = tarjetaReq.IdEntidadFinanciera,
                    EsCredito = tarjetaReq.EsCredito,
                    NombreEnTarjeta = tarjetaReq.NombreEnTarjeta,
                    NumeroTarjeta = tarjetaReq.NumeroTarjeta,
                    Expiracion = tarjetaReq.Expiracion,
                    FechaCreacion = DateTime.Now,
                    Borrado = false,
                    Activo = true
                };

                _context.Tarjetas.Add(tarjeta);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest("No coinciden los parámetros de entrada");
        }

        [HttpPost]
        [Route("api/medios/crear/Cuenta")]
        public IActionResult CrearCuentaUsuario(CuentaRequest cuentaReq)
        {
            var cuentaAux = from m in _context.Cuentas
                             where m.NumeroDeCuenta == cuentaReq.NumeroDeCuenta &&
                             m.IdEntidadFinanciera == cuentaReq.IdEntidadFinanciera &&
                             m.UserId == cuentaReq.IdUser
                             select m;

            if (cuentaAux.Count() > 0)
            {
                return BadRequest("Ya existe una cuenta con ese número para esa entidad financiera");

            }

            if (ModelState.IsValid)
            {

                var cuenta = new Cuenta
                {
                    UserId = cuentaReq.IdUser,
                    Detalle = cuentaReq.Detalle,
                    IdEntidadFinanciera = cuentaReq.IdEntidadFinanciera,
                    NumeroDeCuenta = cuentaReq.NumeroDeCuenta,
                    Sucursal = cuentaReq.Sucursal,
                    FechaCreacion = DateTime.Now,
                    Borrado = false,
                    Activo = true
                };

                _context.Cuentas.Add(cuenta);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest("No coinciden los parámetros de entrada");
        }

        [HttpPost]
        [Route("api/medios/crear/PayPal")]
        public IActionResult CrearPayPalUsuario(PayPalRequest paypalReq)
        {

            var cuentaAux = from m in _context.Paypals
                            where m.CorreoPaypal == paypalReq.CorreoPaypal &&
                            m.UserId == paypalReq.IdUser
                            select m;

            if (cuentaAux.Count() > 0)
            {
                return BadRequest("Ya existe una cuenta PayPal con ese correo");

            }

            //a arreglar
            var EntidadFinancieraAux = _context.EntidadesFinancieras.Select(m => m.Id).First();

            if (ModelState.IsValid)
            {
                var paypal = new PayPal
                {
                    Detalle = paypalReq.Detalle,
                    CorreoPaypal = paypalReq.CorreoPaypal,
                    UserId = paypalReq.IdUser,
                    FechaCreacion = DateTime.Now,
                    IdEntidadFinanciera = EntidadFinancieraAux,
                    Borrado = false,
                    Activo = true
                };

                _context.Paypals.Add(paypal);
                _context.SaveChanges();
                return Ok();
            }

            return BadRequest("No coinciden los parámetros de entrada");
        }

        // PUT api/<ValuesController>/5
        [HttpPost]
        [Route("api/medios/editar/Tarjeta")]
        public async Task<IActionResult> EditarTarjetaUsuarioAsync(TarjetaRequest tarjetaReq)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var tarjetaAux = from m in _context.Tarjetas
                             where m.NumeroTarjeta == tarjetaReq.NumeroTarjeta &&
                             m.IdEntidadFinanciera == tarjetaReq.IdEntidadFinanciera &&
                             m.Id != tarjetaReq.Id &&
                             m.UserId == user.Id
                             select m;

            if (tarjetaAux.Count() > 0)
            {
                return BadRequest("Ya existe una tarjeta con ese número para esa entidad financiera");

            }

            var medio = _context.Tarjetas.Find(tarjetaReq.Id);

            if (ModelState.IsValid)
            {

                medio.Detalle = tarjetaReq.Detalle;
                medio.IdEntidadFinanciera = tarjetaReq.IdEntidadFinanciera;
                medio.EsCredito = tarjetaReq.EsCredito;
                medio.NombreEnTarjeta = tarjetaReq.NombreEnTarjeta;
                medio.NumeroTarjeta = tarjetaReq.NumeroTarjeta;
                medio.Expiracion = tarjetaReq.Expiracion;

                try
                {
                    _context.MediosDePagos.Update(medio);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error al guardar");
                }


            }

            return BadRequest("No coinciden los parámetros de entrada");
        }

        [HttpPost]
        [Route("api/medios/editar/Cuenta")]
        public async Task<IActionResult> EditarCuentaUsuarioAsync(CuentaRequest cuentaReq)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var cuentaAux = from m in _context.Cuentas
                            where m.NumeroDeCuenta == cuentaReq.NumeroDeCuenta &&
                            m.IdEntidadFinanciera == cuentaReq.IdEntidadFinanciera &&
                            m.Id != cuentaReq.Id &&
                            m.UserId == user.Id
                            select m;

            if (cuentaAux.Count() > 0)
            {
                return BadRequest("Ya existe una cuenta con ese número para esa entidad financiera");

            }

            var medio = _context.Cuentas.Find(cuentaReq.Id);

            if (ModelState.IsValid)
            {
                medio.Detalle = cuentaReq.Detalle;
                medio.IdEntidadFinanciera = cuentaReq.IdEntidadFinanciera;
                medio.NumeroDeCuenta = cuentaReq.NumeroDeCuenta;
                medio.Sucursal = cuentaReq.Sucursal;


                _context.MediosDePagos.Update(medio);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("No coinciden los parámetros de entrada");
        }

        [HttpPost]
        [Route("api/medios/editar/PayPal")]
        public async Task<IActionResult> EditarPayPalUsuarioAsync(PayPalRequest paypalReq)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            var cuentaAux = from m in _context.Paypals
                            where m.CorreoPaypal == paypalReq.CorreoPaypal &&
                            m.Id != paypalReq.Id &&
                            m.UserId == user.Id
                            select m;

            if (cuentaAux.Count() > 0)
            {
                return BadRequest("Ya existe una cuenta PayPal con ese correo");

            }
            var medio = _context.Paypals.Find(paypalReq.Id);

            if (ModelState.IsValid)
            {

                medio.Detalle = paypalReq.Detalle;
                medio.CorreoPaypal = paypalReq.CorreoPaypal;

                _context.Paypals.Update(medio);
                await _context.SaveChangesAsync();
                return Ok();
            }

            return BadRequest("No coinciden los parámetros de entrada");
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        [Route("api/medios/borrar/{id}")]
        public async Task BorrarAsync(int id)
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

            medio.Borrado = true;
            _context.MediosDePagos.Update(medio);
            await _context.SaveChangesAsync();
            Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete]
        [Route("api/entidad_financiera/borrar/{id}")]
        public async Task BorrarEntidadFinancieraAsync(int id)
        {
            if (id == 0)
            {
                NotFound();
            }

            //obtener la entidad
            var entidad = _context.EntidadesFinancieras.Find(id);

            if (entidad == null)
            {
                NotFound();
            }

            entidad.Borrado = true;
            _context.EntidadesFinancieras.Update(entidad);
            await _context.SaveChangesAsync();
            Ok();
        }

        // Post api/<ValuesController>/StringSearch
        [HttpPost]
        [Route("api/medios/buscar/{searchString}")]
        public object BusquedaMedioDePago(string searchString)
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
        [HttpPost]
        [Route("api/entidad_financiera/buscar/{searchString}")]
        public object BusquedaEntidadFinanciera(string searchString)
        {
            var entidades = from m in _context.EntidadesFinancieras
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                entidades = entidades.Where(s => s.Nombre.Contains(searchString));
            }

            return JsonConvert.SerializeObject(entidades);

        }
    }
}
