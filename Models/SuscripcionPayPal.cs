using System;
namespace Crud.Models
{
    public class SuscripcionPayPal: PayPal
    {
        public string IdSuscripcionPayPal { get; set; }
        public string IdPlanPayPal { get; set; }
        public string NombrePlan { get; set; }
        public string Frequencia { get; set; }
        public string CantidadFrequencia { get; set; }
        public DateTime FechaInicioSuscripcion { get; set; }
        public string EstadoSuscripcionPayPal { get; set; }
    }
}
