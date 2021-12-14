using System;
namespace Crud.DTOs
{
    public class PagosRequest
    {
        public int Id { get; set; }
        public int MedioDePagoId { get; set; }
        public Decimal Monto { get; set; }
        public string Moneda { get; set; }
        public bool Devolucion { get; set; }
        public bool EsSuscripcion { get; set; }
        public bool EsPayPal { get; set; }
        public bool Aprobado { get; set; }
        public string OrderId { get; set; }
        public string IdCaptura { get; set; }
        public string DevolucionId { get; set; }
        public string EstadoTransaccion { get; set; }
        public int TipoSuscripcionId { get; set; }
        public string ObservacionDevolucion { get; set; }
    }


    public class PagosResponse
    {
        public int IdPago { get; set; }
        public DateTime Fecha { get; set; }
        public bool Aprobado { get; set; }
        public Decimal Monto { get; set; }
        public string Moneda { get; set; }
        public bool Devolucion { get; set; }
        public bool EsSuscripcion { get; set; }
        public int IdPagoDevolucion { get; set; }
        public bool EsPayPal { get; set; }
        public string IdCaptura { get; set; }
        public string NombreUsuario { get; set; }
        public string DetalleSuscripcion { get; set; }
    }

    public class GananciaCreador
    {
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Usuario { get; set; }
        public DateTime FechaMes { get; set; }
        public decimal Monto { get; set; }
        public string EntidadFinanciera { get; set; }
        public string NumeroDeCuenta { get; set; }
    }
}
