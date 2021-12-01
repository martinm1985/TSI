using Crud.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.DTOs
{
    public class ContenidoDto
    {
        public record ContenidoRegistro
        {
            public string Id { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string DerechoAutor { get; set; }

            public string Archivo { get; set; }
            public string Calidad { get; set; }
            public string Duracion { get; set; }
            public int? CategoriaId { get; set; }
            public int? TipoSuscripcionId { get; set; }
            public int Largo { get; set; }
            public string Texto { get; set; }
            public DateTime FechaInicio { get; set; }
            public DateTime FechaFin { get; set; }
            public string Url { get; set; }

        }

        public record GetContenidoData
        {
            public int Id { get; set; }
            public string Titulo { get; set; }
            public string Username { get; set; }
            public string FechaCreacion { get; set; }
            public string Descripcion { get; set; }
            public string Archivo { get; set; }
            public string ArchivoContenido { get; set; }
            public string Calidad { get; set; }
            public Decimal Duracion { get; set; }
            public Decimal DuracionVideo { get; set; }
            public int? CategoriaId { get; set; }
            public string CategoriaNombre { get; set; }
            public int Largo { get; set; }
            public string Texto { get; set; }
            public string FechaInicio { get; set; }
            public string FechaFin { get; set; }
            public string Url { get; set; }

        }


        public record ContenidoCast
        {
            public int Cast { get; set; }
            public string Descripcion { get; set; }

        }

        public record GetAllContenido
        {
            public int Id { get; set; }
            public string CreadorId { get; set; }
            public string Username { get; set; }
            public string Titulo { get; set; }
            public string Descripcion { get; set; }
            public string FechaCreacion { get; set; }
            public int? CategoriaId { get; set; }
            public string CategoriaNombre { get; set; }
            public string Texto { get; set; }
            public int? Largo { get; set; }
            public string Archivo { get; set; }
            public string Calidad { get; set; }
            public Decimal? Duracion { get; set; }
            public Decimal? DuracionVideo { get; set; }
            public string Url { get; set; }
            public string FechaInicio { get; set; }
            public string FechaFin { get; set; }
            public int? SuscripcionId { get; set; }
        }
                           
    }
}
