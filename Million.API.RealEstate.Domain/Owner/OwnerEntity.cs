using System;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.API.RealEstate.Domain.Owner
{
    public class OwnerEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Photo { get; set; } // Puede ser URL o base64

        public DateTime BirthDay { get; set; }
    }
}
