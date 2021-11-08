using System;
namespace Crud.Models
{
    public class PayPal: MediosDePagos
    {
        public string ClientIdPayPal { get; set; }
        public string CorreoPaypal { get; set; }
    }
}
