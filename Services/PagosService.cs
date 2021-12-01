using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using Crud.Models;
using Crud.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Crud.Services
{
    public class PagosService : IPagoService
    {
        private readonly ApplicationDbContext _context;

        public PagosService(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task ActualizarFinanzaPagoAsync(decimal monto, int idTipoSuscripcion)
        {
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

    }
}