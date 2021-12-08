
using System;

namespace Crud.DTOs
{
    public class PayPalRequest
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Detalle { get; set; }
        public string CorreoPaypal { get; set; }
        public bool Activo { get; set; }

    }

    public class TarjetaRequest
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Detalle { get; set; }
        public int IdEntidadFinanciera { get; set; }
        public bool EsCredito { get; set; }
        public string NombreEnTarjeta { get; set; }
        public string NumeroTarjeta { get; set; }
        public DateTime Expiracion { get; set; }
        public bool Activo { get; set; }
    }

    public class CuentaRequest
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Detalle { get; set; }
        public int IdEntidadFinanciera { get; set; }
        public string NumeroDeCuenta { get; set; }
        public string Sucursal { get; set; }
        public bool Activo { get; set; }

    }

    public class EntidadFinancieraRequest
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
        public bool TarjetaCredito { get; set; }
        public bool TarjetaDebito { get; set; }
        public bool Cuenta { get; set; }
        public bool Activo { get; set; }

    }

    public class TarjetaResponse
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Detalle { get; set; }
        public int IdEntidadFinanciera { get; set; }
        public string EntidadFinanciera { get; set; }
        public bool EsCredito { get; set; }
        public string NombreEnTarjeta { get; set; }
        public string NumeroTarjeta { get; set; }
        public DateTime Expiracion { get; set; }
        public bool Activo { get; set; }
    }

    public class CuentaResponse
    {
        public int Id { get; set; }
        public string IdUser { get; set; }
        public string Detalle { get; set; }
        public int IdEntidadFinanciera { get; set; }
        public string EntidadFinanciera { get; set; }
        public string NumeroDeCuenta { get; set; }
        public string Sucursal { get; set; }
        public bool Activo { get; set; }

    }
}