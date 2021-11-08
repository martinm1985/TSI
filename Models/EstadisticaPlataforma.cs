using System;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Crud.Models
{
    public class EstadisticaPlataforma
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaMes { get; set; }
        public int CantSeguidores { get; set; }
        public int CantSuscripciones { get; set; }
        public int CantVisitasPerfil { get; set; }
    }
}
