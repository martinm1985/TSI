using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{


    public class Audio : Contenido
    {
        [Column(TypeName = "decimal(18,2)")]
        public Decimal Duracion { get; set; }


    }

}