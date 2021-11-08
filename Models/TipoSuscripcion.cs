using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models
{
    public class TipoSuscripcion
    {
        [Key]
        public int Id { get; set; } 
        [ForeignKey("Creador")]
        public string CreadorId {get; set;}
        public Creador Creador { get; set; }

        public string Nombre { get; set; }

        public float Precio { get; set; }

        public string Imagen { get; set; }

        public string Beneficios { get; set; }

        public string MensajeBienvenida { get; set; }

        public bool VideoBienvenida { get; set;  }

        public bool MensajeriaActiva {  get; set; }

        public bool Activo {  get; set; }

        public int? IncluyeTipoSuscrId { get; set; }


        [ForeignKey("IncluyeTipoSuscrId")]
        public virtual TipoSuscripcion IncluyeTipoSuscr { get; set; }

        public virtual ICollection<SuscripcionUsuario> SuscripcionesUsuarios { get; set; }
        public virtual ICollection<HistoricoSuscripcion> HistoricoSuscripciones { get; set; }
    }
}
