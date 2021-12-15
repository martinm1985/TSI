using Microsoft.AspNetCore.Mvc;
using System;
using Crud.Models;
using Crud.Data;
using Crud.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using AutoMapper;
using Crud.Services;

namespace Crud.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class PagosApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<User> _userManager;

        private readonly IPagoService _pagosService;

        public PagosApiController(ApplicationDbContext context, UserManager<User> userManager, IPagoService pagosService)
        {
            _context = context;
            _userManager = userManager;
            _pagosService = pagosService;
        }

        [HttpPost]
        [Route("api/pagos/registrarPago")]
        public async Task<IActionResult> RegistrarPago(PagosRequest pago)
        {
            if (ModelState.IsValid)
            {
                var registroPago = new Pago
                {
                    IdMedioDePago = pago.MedioDePagoId,
                    Fecha = DateTime.Now,
                    //si es PayPal ya se cobró en caso contrario se debe cobrar
                    Aprobado = pago.EsPayPal,
                    Monto = pago.Monto,
                    Moneda = pago.Moneda,
                    Devolucion = pago.Devolucion,
                    EsSuscripcion = pago.EsSuscripcion,
                    EsPayPal = pago.EsPayPal,
                    TipoSuscripcionId = pago.TipoSuscripcionId,
                };

                try
                {
                    _context.Pagos.Add(registroPago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error al registrar el pago");
                }


                if (pago.EsPayPal)
                {

                    var pagoPayPal = new PagoPayPal
                    {
                        PagoId = registroPago.IdPago,
                        OrderId = pago.OrderId,
                        IdCaptura = pago.IdCaptura,
                        EstadoPago = pago.EstadoTransaccion,
                        FechaPago = registroPago.Fecha
                    };

                    _context.PagosPayPal.Add(pagoPayPal);
                }

                try
                {
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error al registrar PayPal");
                }

            }

            return BadRequest("No coinciden los parámetros de entrada");
        }


        [HttpPost]
        [Route("api/pagos/devolverPago")]
        public async Task<IActionResult> DevolverPago(PagosRequest devolucion)
        {

            if (ModelState.IsValid)
            {
                var porcentaje = decimal.Parse(await _context.Parametros
                .Where(m => m.Nombre == "GananciaCreador")
                .Select(m => m.Valor).FirstAsync());

                var pago = await _context.Pagos.Where(m => m.IdPago == devolucion.Id).FirstAsync();
                pago.Devuelto = true;
                _context.Pagos.Update(pago);

                var registroDevolucion = new Pago
                {
                    IdMedioDePago = pago.IdMedioDePago,
                    Fecha = DateTime.Now,
                    //si es PayPal ya se devolvió en caso contrario se debe cobrar
                    Aprobado = pago.EsPayPal,
                    Monto = pago.Monto*porcentaje,
                    Moneda = pago.Moneda,
                    Devolucion = true,
                    EsSuscripcion = pago.EsSuscripcion,
                    EsPayPal = pago.EsPayPal,
                    //el IdPago asociado a la devolución
                    IdPagoDevolucion = pago.IdPago,
                    ObservacionDevolucion = devolucion.ObservacionDevolucion,
                    TipoSuscripcionId = pago.TipoSuscripcionId
                };

                try
                {
                    _context.Pagos.Add(registroDevolucion);
                    await _pagosService.ActualizarFinanzaDevolucionAsync(pago.IdPago);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error al registrar el pago");
                }


                if (pago.EsPayPal)
                {

                    var devolucionPayPal = new DevolucionPayPal
                    {
                        PagoId = registroDevolucion.IdPago,
                        DevolucionId = devolucion.DevolucionId,
                        EstadoDevolucion = devolucion.EstadoTransaccion,
                        FechaDevolucion = registroDevolucion.Fecha
                    };

                    _context.DevolucionesPayPal.Add(devolucionPayPal);
                }

                try
                {

                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return BadRequest("Error al registrar PayPal");
                }


            }

            return BadRequest("No coinciden los parámetros de entrada");


        }


        [HttpGet]
        [Route("api/pagos/getPagosPayPal")]
        public async Task<ActionResult<IEnumerable<PagosResponse>>> GetPagosPayPal()
        {
            var creador = await _userManager.GetUserAsync(HttpContext.User);

            var porcentaje = decimal.Parse(await _context.Parametros
                .Where(m => m.Nombre == "GananciaCreador")
                .Select(m => m.Valor).FirstAsync());

            //obtener los pagos para este creador que no son PayPal
            var query = from p in _context.Pagos
                        join t in _context.TipoSuscripcion on p.TipoSuscripcionId equals t.Id
                        join m in _context.MediosDePagos on p.IdMedioDePago equals m.Id
                        join u in _context.Users on m.UserId equals u.Id
                        join pp in _context.PagosPayPal on p.IdPago equals pp.PagoId
                        where t.CreadorId == creador.Id && p.Aprobado && !p.Devolucion
                        && p.EsPayPal && !p.Devuelto
                        select new PagosResponse
                        {
                            IdPago = p.IdPago,
                            Fecha = p.Fecha,
                            Aprobado = p.Aprobado,
                            Monto = p.Monto * porcentaje,
                            Moneda = p.Moneda,
                            Devolucion = p.Devolucion,
                            EsSuscripcion = p.EsSuscripcion,
                            IdPagoDevolucion = p.IdPagoDevolucion,
                            EsPayPal = p.EsPayPal,
                            IdCaptura = pp.IdCaptura,
                            NombreUsuario = u.UserName + "/" + u.Name + " " + u.Surname,
                            DetalleSuscripcion = t.Nombre
                        };

            if (query == null)
            {
                return NotFound();
            }

            return Ok(query.ToList());

        }

        [HttpGet]
        [Route("api/pagos/getPagosNoPayPal")]
        public async Task<ActionResult<IEnumerable<PagosResponse>>> GetPagosNoPayPal()
        {
            var creador = await _userManager.GetUserAsync(HttpContext.User);

            var porcentaje = decimal.Parse(await _context.Parametros
                .Where(m => m.Nombre == "GananciaCreador")
                .Select(m=>m.Valor).FirstAsync());

            //obtener los pagos para este creador que no son PayPal
            var query = from p in _context.Pagos
                        join t in _context.TipoSuscripcion on p.TipoSuscripcionId equals t.Id
                        join m in _context.MediosDePagos on p.IdMedioDePago equals m.Id
                        join u in _context.Users on m.UserId equals u.Id
                        where t.CreadorId == creador.Id && p.Aprobado && !p.Devolucion
                        && !p.EsPayPal && !p.Devuelto
                        select new PagosResponse
                        {
                            IdPago = p.IdPago,
                            Fecha = p.Fecha,
                            Aprobado = p.Aprobado,
                            Monto = p.Monto*porcentaje,
                            Moneda = p.Moneda,
                            Devolucion = p.Devolucion,
                            EsSuscripcion = p.EsSuscripcion,
                            IdPagoDevolucion = p.IdPagoDevolucion,
                            EsPayPal = p.EsPayPal,
                            NombreUsuario = u.UserName + "/" + u.Name + " " + u.Surname,
                            DetalleSuscripcion = t.Nombre
                        };

            if (query == null)
            {
                return NotFound();
            }

            return Ok(query.ToList());

        }

        [HttpGet]
        [Route("api/pagos/getGanaciasCreadores")]
        public async Task<ActionResult<IEnumerable<GananciaCreador>>> GetGananciasCreadores()
        {

                //obtener los pagos para este creador que no son PayPal
                var query = from p in _context.Creadores
                            join f in _context.Finanza on p.Id equals f.CreadorId
                            select new GananciaCreador
                            {
                                Nombre = p.Usuario.Name,
                                Apellido = p.Usuario.Surname,
                                Usuario = p.Usuario.UserName,
                                FechaMes = f.FechaMes,
                                Monto = f.Monto,
                                EntidadFinanciera = p.EntidadFinanciera,
                                NumeroDeCuenta = p.NumeroDeCuenta
                            };

                if (query == null)
                {
                    return NotFound();
                }

                return Ok(await query.ToListAsync());

        }


    }

  
}
