using System;
namespace Crud.Models
{
    public class Tarjeta : MediosDePagos
    {
        public string NombreEnTarjeta { get; set; }

        public bool EsCredito { get; set; }

        public string NumeroTarjeta { get; set; }

        public DateTime Expiracion { get; set; }
    }
}
 