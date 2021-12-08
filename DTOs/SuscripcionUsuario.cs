using Crud.Models;
using System;
using static Crud.DTOs.SuscripcionDto;


namespace Crud.DTOs
{
    public class SuscripcionUsuarioDto
    {
        public int Id { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        public bool Activo { get; set; }

        public int TipoSuscripcionId { get; set; }

        public virtual TipoSuscripcionDto TipoSuscripcion { get; set; }
        public string UsuarioId { get; set; }
        public UserData Usuario { get; set; }

        public int MedioDePagoId { get; set; }

    }
}
