using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.ComponentModel.DataAnnotations;
using RentAMotorcycleRabbitMQ.Utils;

namespace RentAMotorcycleRabbitMQ.Models
{
    [BsonIgnoreExtraElements]
    [BsonConnection(0, "rent_motors", "motors")]
    public class Motors
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [Required]
        public int Identificador { get; set; }
        [Required]
        public int Ano { get; set; }
        [Required]
        public string Modelo { get; set; }

        [Required]
        public string Placa { get; set; }

        public DateTime DateAdd { get; set; }
        public DateTime DateUpd { get; set; }
        public bool IsActive { get; set; }
    }
}
