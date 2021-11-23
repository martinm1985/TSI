using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class PagoPayPal
    {
        public string IdPagoPaypal {get; set;}
        public string Invoice_id { get; set;}
        public string IdAutorizacion { get; set; }
        public string IdCaptura { get; set; }
        public string Estado { get; set;}
        public DateTime FechaTransaccion { get; set; }
        public string IdReembolso { get; set; }

    }
}
