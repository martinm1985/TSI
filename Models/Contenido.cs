using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class Contenido
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Creador")]
        public string CreadorId {get; set;}
        public Creador Creador { get; set; }
        [Required(ErrorMessage = "El titulo es obligatorio")]
        public string Titulo { get; set; }

        public string Descripcion { get; set; }
        [Required(ErrorMessage = "La fecha es obligatorio")]
        [DataType(DataType.Date)]
        public DateTime FechaCreacion { get; set; }

        public bool Bloqueado { get; set; }

        public string DerechoAutor { get; set; }
        public byte[] Archivo { get; set; }
        public string Calidad { get; set; }
        public int? CategoriaId { get; set; }
        [ForeignKey("CategoriaId")]
        public virtual Categoria Categoria { get; set; }
        public int? TipoSuscripcionId { get; set; }
        [ForeignKey("TipoSuscripcionId")]
        public virtual TipoSuscripcion TipoSuscripcion { get; set; }
    }
}
