using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;


namespace Crud.Models
{
    public class Mensaje
    {
        [Key]
        public int MensajeId {  get; set; }

        [Required(ErrorMessage = "Se debe agregar una fecha")]
        public DateTime FechaHora {  get; set; }

        [Required(ErrorMessage = "El mensaje no puede ser vacio")]
        public string CuerpoMensaje {  get; set; }

        public Boolean Leido { get; set; }

        [ForeignKey("ConversacionId")]
        public int ConversacionId { get; set; }

        public string UserSender { get; set; }

    }
}
