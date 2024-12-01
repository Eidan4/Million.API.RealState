using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.API.RealEstate.Domain.Property
{
    public class PropertyEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public double Price { get; set; }

        public string CodeInternal { get; set; }

        public int Year { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdOwner { get; set; } // Referencia al Owner
    }
}
