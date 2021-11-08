using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class Parametros
    {
        [Key]
        public string Nombre { get; set; }
        public string Valor { get; set; }
    }
}
