using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class HistoricoSuscripcion
    {

        [Key]
        public int HistoricoSuscripcionId {  get; set; }
        public int TipoSuscripcionId { get; set; }
        [ForeignKey("TipoSuscripcionId")]
        public virtual TipoSuscripcion TipoSuscripcion { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaFin { get; set; }

        public float Precio { get; set; }

    }
}
