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
                    IdMedioDePago = pago.IdMedioDePago,
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
                    await _pagosService.ActualizarFinanzaPagoAsync(pago.Monto, pago.TipoSuscripcionId);
                  
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

                var pago = _context.Pagos.Where(m => m.IdPago == devolucion.Id).First();

                var registroDevolucion = new Pago
                {
                    IdMedioDePago = pago.IdMedioDePago,
                    Fecha = DateTime.Now,
                    //si es PayPal ya se devolvió en caso contrario se debe cobrar
                    Aprobado = pago.EsPayPal,
                    Monto = pago.Monto,
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
        [Route("api/medios/getPagosPayPal")]
        public async Task<ActionResult<IEnumerable<PagosResponse>>> GetPagosPayPal()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            //obtener los pagos de este usuario por medio de PayPal
            var query = from p in _context.Pagos
                        join medio in _context.MediosDePagos on p.IdMedioDePago equals medio.Id
                        join paypal in _context.PagosPayPal on p.IdPago equals paypal.PagoId
                        where medio.UserId == user.Id && !p.Devolucion
                        select new PagosResponse {
                            IdPago = p.IdPago,
                            Fecha = p.Fecha,
                            Aprobado = p.Aprobado,
                            Monto = p.Monto,
                            Moneda = p.Moneda,
                            Devolucion = p.Devolucion,
                            EsSuscripcion = p.EsSuscripcion,
                            IdPagoDevolucion = p.IdPagoDevolucion,
                            EsPayPal = p.EsPayPal,
                            IdCaptura = paypal.IdCaptura
                        };

            if (query == null)
            {
                return NotFound();
            }

            return Ok(query.ToList());

        }

        [HttpGet]
        [Route("api/medios/getPagosNoPayPal")]
        public async Task<ActionResult<IEnumerable<PagosResponse>>> GetPagosNoPayPal()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);

            //obtener los pagos de este usuario que no son PayPal
            var query = from p in _context.Pagos
                        join medio in _context.MediosDePagos on p.IdMedioDePago equals medio.Id
                        where medio.UserId == user.Id && !p.Devolucion && !p.EsPayPal
                        select new PagosResponse
                        {
                            IdPago = p.IdPago,
                            Fecha = p.Fecha,
                            Aprobado = p.Aprobado,
                            Monto = p.Monto,
                            Moneda = p.Moneda,
                            Devolucion = p.Devolucion,
                            EsSuscripcion = p.EsSuscripcion,
                            IdPagoDevolucion = p.IdPagoDevolucion,
                            EsPayPal = p.EsPayPal
                        };

            if (query == null)
            {
                return NotFound();
            }

            return Ok(query.ToList());

        }

    }

}
