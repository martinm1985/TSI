using System;
using System.ComponentModel.DataAnnotations;

namespace Crud.Models
{
    public class EntidadFinanciera
    {
        [Key]
        public int Id { get; set; }

        public string Nombre { get; set; }

        public string Direccion { get; set; }

        public string Telefono { get; set; }

        public bool TarjetaCredito { get; set; }

        public bool TarjetaDebito { get; set; }

        public bool Cuenta { get; set; }

        public bool Borrado { get; set; }
    }
}
