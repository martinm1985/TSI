using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Crud.Models
{
    public class SuscripcionUsuario
    {
        public int Id { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool Activo { get; set; }

        public int TipoSuscripcionId { get; set; }

        [ForeignKey("TipoSuscripcionId")]
        public virtual TipoSuscripcion TipoSuscripcion { get; set; }
        [ForeignKey("Usuario")]
        public string UsuarioId { get; set; }
        public User Usuario { get; set; }

        [ForeignKey("MediosDePagos")]
        public int MedioDePagoId { get; set; }

        public virtual MediosDePagos MedioDePagoUsuario {get; set;}

    }
}
