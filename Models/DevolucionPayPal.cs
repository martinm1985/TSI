using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models
{
    public class DevolucionPayPal
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pago")]
        public int PagoId { get; set; }

        public virtual Pago Pago { get; set; }

        public string DevolucionId { get; set; }

        public string EstadoDevolucion { get; set; }

        public DateTime FechaDevolucion{ get; set; }
    }
}
