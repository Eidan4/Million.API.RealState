using Million.API.RealEstate.Domain.Owner;
using Million.API.RealEstate.Domain.Property;
using Million.API.RealEstate.Domain.PropertyImage;
using Million.API.RealEstate.Domain.PropertyTrace;
using MongoDB.Driver;

namespace Million.API.RealEstate.Persistence.DBContext
{
    public class RealEstateDbContext
    {
        private readonly IMongoDatabase _database;

        public RealEstateDbContext(IMongoClient mongoClient)
        {
            // Nombre de la base de datos, puede obtenerse de una configuración
            _database = mongoClient.GetDatabase("RealEstateDb");
        }

        #region DBSets
        public IMongoCollection<OwnerEntity> Owners => _database.GetCollection<OwnerEntity>("Owner");
        public IMongoCollection<PropertyEntity> Properties => _database.GetCollection<PropertyEntity>("Property");
        public IMongoCollection<PropertyImageEntity> PropertyImages => _database.GetCollection<PropertyImageEntity>("PropertyImage");
        public IMongoCollection<PropertyTraceEntity> PropertyTraces => _database.GetCollection<PropertyTraceEntity>("PropertyTrace");
        #endregion
    }
}
