using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.DTOs;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Crud.Models;

namespace Crud.Services
{
    public interface IPagoService
    {
        public Task ActualizarFinanzaPagoAsync(decimal monto, int idTipoSuscripcion);

        public Task ActualizarFinanzaDevolucionAsync(int idPago);

        public Task CobroDePagos();

        public Task ActualizacionSuscripciones();

        public Task ActualizacionTarjetas();

        public Task SuscripcionesNoPagas();
    }
}
