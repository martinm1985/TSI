using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class MediosDePagos
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage ="se debe agregar un detalle")]
        [StringLength(50, ErrorMessage = "debe tener mínimo {2} maximo {1}", MinimumLength = 3)]
        [Display(Name = "Detalle")]
        public string Detalle { get; set; }


        [Required(ErrorMessage = "se debe agregar una fecha")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Validez")]
        public DateTime FechaValidez { get; set; }


        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; }

        [ForeignKey("Pago")]
        public int IdPago { get; set; }
    }
}
