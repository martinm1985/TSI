using System;
namespace Crud.Models
{
    public class Cuenta : MediosDePagos
    {
        public string NumeroDeCuenta { get; set; }

        public string Sucursal { get; set; }
    }
}
