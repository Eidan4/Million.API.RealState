using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.API.RealEstate.Domain.PropertyTrace
{
    public class PropertyTraceEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public DateTime DataSale { get; set; }

        public string Name { get; set; }

        public double Value { get; set; }

        public double Tax { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public string IdProperty { get; set; } // Referencia al Property
    }
}
