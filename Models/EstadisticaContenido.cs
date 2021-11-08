using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Crud.Models
{
    public class EstadisticaContenido
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string IdContenido {  get; set;}
        [DataType(DataType.Date)]
        public DateTime Fecha {  get; set; }
        public int Visualizaciones {  get; set; }
    }
}
