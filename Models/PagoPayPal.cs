using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models
{
    public class PagoPayPal
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Pago")]
        public int PagoId { get; set; }

        public virtual Pago Pago { get; set; }

        public string OrderId {get; set;}

        public string IdCaptura { get; set; }

        public string EstadoPago { get; set;}

        public DateTime FechaPago { get; set; }
    }
}
