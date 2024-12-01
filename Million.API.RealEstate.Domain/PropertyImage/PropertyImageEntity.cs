using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.API.RealEstate.Domain.PropertyImage
{
    public class PropertyImageEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; } // Referencia al Property

        public string File { get; set; } // Puede ser URL o base64

        public bool Enabled { get; set; }
    }
}
