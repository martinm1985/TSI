using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class Conversacion
    {

        [Key]
        public int ConversacionId { get; set; }
        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public User Usuario { get; set; }
        [ForeignKey("Creador")]
        public string CreadorId {get; set;}
        public Creador Creador { get; set; }

        [DataType(DataType.Date)]
        public DateTime FechaInicio { get; set; }

        public virtual ICollection<Mensaje> Mensajes {  get; set; }

        
    }
}
