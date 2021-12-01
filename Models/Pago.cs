using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class Pago
    {
        [Key]
        public int IdPago { get; set; }

        [ForeignKey("MediosDePagos")]
        public int IdMedioDePago { get; set; }

        public virtual MediosDePagos Medio { get; set; }

        public DateTime Fecha { get; set; }

        public bool Aprobado { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public Decimal Monto { get; set; } = 0;

        public string Moneda { get; set; }

        public bool Devolucion { get; set; }

        public bool EsSuscripcion { get; set; }

        public int IdPagoDevolucion { get; set; }

        public bool EsPayPal { get; set; }

        [ForeignKey("TipoSuscripcion")]
        public int TipoSuscripcionId { get; set; }

        public virtual TipoSuscripcion Suscripcion {get; set;}

        public string ObservacionDevolucion { get; set; }
    }
}
