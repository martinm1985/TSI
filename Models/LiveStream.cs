using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{


    public class LiveStream : Contenido
    {

        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }


}

}