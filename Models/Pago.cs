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
        public DateTime Fecha { get; set; }
        public bool Aprobado { get; set; }
        public float Monto { get; set; }
        public string Moneda { get; set; }
        public bool Devolucion { get; set; }
        public bool EsSuscripcion { get; set; }
    }
}
