using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class Finanza
    {
        
        [Key]
        public int FinanzaId { get; set; }

        [Required(ErrorMessage = "Se debe agregar una fecha")]
        [DataType(DataType.Date)]
        public DateTime FechaMes {  get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public Decimal Monto { get; set; } = 0;

        public int CantidadDevoluciones { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public Decimal Indicador { get; set; }
    }
}
