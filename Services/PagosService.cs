using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Crud.Models;
using Crud.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Quartz;
using Crud.Scheduler;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Crud.Services
{
    public class PagosService : IPagoService
    {
        private readonly IServiceScopeFactory _scopeFactory;

        public PagosService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }


        public async Task ActualizarFinanzaPagoAsync(decimal monto, int idTipoSuscripcion)
        {
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            _context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var finanzaQuery = (from f in _context.Finanza
                            join s in _context.TipoSuscripcion on f.CreadorId equals s.CreadorId
                            where s.Id == idTipoSuscripcion && (f.FechaMes.Month == DateTime.Now.Month
                            && f.FechaMes.Year == DateTime.Now.Year)
                                select f);

            var porcentaje = decimal.Parse(_context.Parametros.Find("GananciaCreador").Valor);

            if (finanzaQuery.Count() == 0)
            {
                var nuevaFinanza = new Finanza
                {
                    FechaMes = DateTime.Now,

                    Monto = monto * porcentaje,

                    CantidadDevoluciones = 0,

                    Indicador = 0,

                    CreadorId = _context.TipoSuscripcion.Where(m => m.Id == idTipoSuscripcion).FirstOrDefault().CreadorId
                };

                _context.Finanza.Add(nuevaFinanza);

            }
            else
            {

                var finanza = await _context.Finanza.FindAsync(finanzaQuery.First().FinanzaId);

                finanza.Monto += monto * porcentaje;

                _context.Finanza.Update(finanza);
            }

            var finanzaPlataforma = (from f in _context.FinanzaPlataforma
                                where (f.FechaMes.Month == DateTime.Now.Month
                                && f.FechaMes.Year == DateTime.Now.Year)
                                     select f);

            if (finanzaPlataforma.Count() == 0)
            {
                var nuevaFinanzaP = new FinanzaPlataforma
                {
                    FechaMes = DateTime.Now,

                    Monto = monto * (1-porcentaje),

                    Indicador = 0,
                };

                _context.FinanzaPlataforma.Add(nuevaFinanzaP);

            }
            else
            {

                var finanzaP = await _context.FinanzaPlataforma.FindAsync(finanzaPlataforma.First().FinanzaPlataformaId);

                finanzaP.Monto += monto * (1-porcentaje);

                _context.FinanzaPlataforma.Update(finanzaP);

            }


            try
            {

                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {
                
            }

        }

        public async Task ActualizarFinanzaDevolucionAsync(int idPago)
        {
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pago = await _context.Pagos.FindAsync(idPago);

            var finanzaQuery = (from f in _context.Finanza
                                join s in _context.TipoSuscripcion on f.CreadorId equals s.CreadorId
                                where s.Id == pago.TipoSuscripcionId && (f.FechaMes.Month == DateTime.Now.Month)
                                select f).First();

            var porcentaje = decimal.Parse(_context.Parametros.Find("GananciaCreador").Valor);

            if (finanzaQuery == null)
            {
                var nuevaFinanza = new Finanza
                {
                    FechaMes = DateTime.Now,

                    Monto = - pago.Monto * porcentaje,

                    CantidadDevoluciones = 1,

                    Indicador = 0,

                    CreadorId = _context.TipoSuscripcion.Where(m => m.Id == pago.TipoSuscripcionId).FirstOrDefault().CreadorId
                };

                _context.Finanza.Add(nuevaFinanza);

            }
            else
            {

                var finanza = await _context.Finanza.FindAsync(finanzaQuery.FinanzaId);
                finanza.Monto -= pago.Monto * porcentaje;
                finanza.CantidadDevoluciones++;
                _context.Entry(finanza).State = EntityState.Modified;
            }

            try
            {

                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {

            }

        }

        public async Task CobroDePagos()
        {
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var pagos = await (from u in _context.Users
                                join m in _context.MediosDePagos on u.Id equals m.UserId
                                join s in _context.SuscripcionUsuario on u.Id equals s.UsuarioId
                                join p in _context.Pagos on s.TipoSuscripcionId equals p.TipoSuscripcionId
                                join t in _context.TipoSuscripcion on s.TipoSuscripcionId equals t.Id
                                where u.LockoutEnd == null && s.Activo && !p.Aprobado && !m.Borrado && m.Activo
                                && !p.EsPayPal
                                && p.Fecha <= DateTime.Now && !p.Devolucion
                                select new {
                                    idPago = p.IdPago,
                                    monto = t.Precio,
                                }).Distinct().ToListAsync();
            var i = 0;

            foreach (var pago in pagos)
            {
                var pagoUpdate = await _context.Pagos.FindAsync(pago.idPago);
                pagoUpdate.Aprobado = true;

                _context.Pagos.Update(pagoUpdate);

                await ActualizarFinanzaPagoAsync(pagoUpdate.Monto, pagoUpdate.TipoSuscripcionId);

                var siguiente = new Pago
                {

                    IdMedioDePago = pagoUpdate.IdMedioDePago,
                    Fecha = pagoUpdate.Fecha.AddMonths(1),
                    Aprobado = false,
                    Monto = (decimal)((double)pago.monto),
                    Moneda = pagoUpdate.Moneda,
                    Devolucion = pagoUpdate.Devolucion,
                    EsSuscripcion = pagoUpdate.EsSuscripcion,
                    IdPagoDevolucion = pagoUpdate.IdPagoDevolucion,
                    EsPayPal = pagoUpdate.EsPayPal,
                    TipoSuscripcionId = pagoUpdate.TipoSuscripcionId,
                    ObservacionDevolucion = pagoUpdate.ObservacionDevolucion
                };
                i++;
                await _context.Pagos.AddAsync(siguiente);
            }

            try
            {
                Console.WriteLine($" Se cobraron y generaron {i} pagos nuevos.");
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {

            }

        }

        public static decimal MonthDifference(DateTime FechaFin, DateTime FechaInicio)
        {
            return Math.Abs((FechaFin.Month - FechaInicio.Month) + 12 * (FechaFin.Year - FechaInicio.Year));

        }

        public async Task SuscripcionesNoPagas()
        {
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var suscripciones = await (from u in _context.Users
                                       join s in _context.SuscripcionUsuario on u.Id equals s.UsuarioId
                                       where u.LockoutEnd == null
                                       && s.FechaFin <= DateTime.Now
                                       select s).Distinct().ToListAsync();
            var i = 0;


            foreach (var suscripcion in suscripciones)
            {
                var meses = (int) Math.Truncate(MonthDifference(DateTime.Now, suscripcion.FechaInicio));
                var fechapago = suscripcion.FechaInicio.AddMonths(meses);

                if ( meses != 0 && (fechapago > DateTime.Now && fechapago < DateTime.Now.AddDays(1)))
                {
                    var query = (from p in _context.Pagos
                                 join m in _context.MediosDePagos on p.IdMedioDePago equals m.Id
                                 where
                                 m.UserId == suscripcion.UsuarioId &&
                                 p.TipoSuscripcionId == suscripcion.TipoSuscripcionId &&
                                 p.Fecha < DateTime.Now && p.Aprobado
                                 select p).CountAsync();

                    if ((await query == meses) && suscripcion.Activo)
                    {
                        var suscripcionUpdate = await _context.SuscripcionUsuario.FindAsync(suscripcion.Id);
                        suscripcionUpdate.Activo = false;
                        _context.SuscripcionUsuario.Update(suscripcionUpdate);
                    }else if (!suscripcion.Activo)
                    {
                        var suscripcionUpdate = await _context.SuscripcionUsuario.FindAsync(suscripcion.Id);
                        suscripcionUpdate.Activo = true;
                        _context.SuscripcionUsuario.Update(suscripcionUpdate);
                    }
                }
                
                i++;
            };

            try
            {
                Console.WriteLine($" Se activaron/desactivaron {i} suscripciones nuevas por pagos.");
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {

            }


        }

        public async Task ActualizacionSuscripciones()
        {
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var suscripciones = await (from u in _context.Users
                               join s in _context.SuscripcionUsuario on u.Id equals s.UsuarioId
                               where u.LockoutEnd == null && s.Activo
                               && s.FechaFin <= DateTime.Now
                               select s.Id).Distinct().ToListAsync();
            var i = 0;

            foreach(var suscripcion in suscripciones)
            {
                var suscripcionUpdate = await _context.SuscripcionUsuario.FindAsync(suscripcion);
                suscripcionUpdate.Activo = false;
                _context.SuscripcionUsuario.Update(suscripcionUpdate);
                i++;
            };

            try
            {
                Console.WriteLine($" Se vencieron {i} suscripciones nuevas.");
                await _context.SaveChangesAsync();

            }
            catch (DbUpdateConcurrencyException)
            {

            }


        }

        public async Task ActualizacionTarjetas()
        {
            using var scope = _scopeFactory.CreateScope();
            var _context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            _context = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var tarjetas = await (from t in _context.Tarjetas
                                  join u in _context.Users on t.UserId equals u.Id
                                  where !t.Borrado && u.LockoutEnd == null
                                  select t.Id).Distinct().ToListAsync();
            var i = 0;

            foreach (var tarjeta in tarjetas)
            {
                var tarjetaUpdate = await _context.Tarjetas.FindAsync(tarjeta);
                var actualizar =
                    tarjetaUpdate.Activo != (tarjetaUpdate.Expiracion.Year > DateTime.Now.Year
                    || (tarjetaUpdate.Expiracion.Year == DateTime.Now.Year
                    && tarjetaUpdate.Expiracion.Month >= DateTime.Now.Month));
                if (actualizar)
                {
                    tarjetaUpdate.Activo = !tarjetaUpdate.Activo;
                    _context.Tarjetas.Update(tarjetaUpdate);
                    i++;
                }
            };
            if (i > 0)
            {
                try
                {
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {

                }

            };

            Console.WriteLine($" Se activaron/vencieron {i} tarjetas nuevas.");

        }


    }
}