using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.DTOs
{
    public class SuscripcionDto
    {
        public record TipoSuscripcionDto
        {
            public int Id { get; set; }

            public string Nombre { get; set; }

            public float Precio { get; set; }

            public string Imagen { get; set; }

            public string Beneficios { get; set; }

            public string MensajeBienvenida { get; set; }

            public bool VideoBienvenida { get; set; }

            public bool MensajeriaActiva { get; set; }

            public bool Activo { get; set; }

            public int? IncluyeTipoSuscrId { get; set; }

            public FileDto.File ImagenFile { get; set; }

        }
    }
}
