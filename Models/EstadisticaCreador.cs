using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;


namespace Crud.Models
{
    public class EstadisticaCreador
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [ForeignKey("Creador")]
        public string CreadorId {get; set;}
        public Creador Creador { get; set; }
        [DataType(DataType.Date)]
        public DateTime FechaMes { get; set; }
        public int CantSeguidores { get; set; }
        public int CantSuscripciones { get; set; }
        public int CantVisitasPerfil { get; set; }
    }
}
