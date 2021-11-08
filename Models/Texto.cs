using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{


    public class Texto : Contenido
    {

        public int Largo { get; set; }

        public string Html { get; set; }

    }

}