using MongoDB.Driver;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Domain.PropertyTrace;
using Million.API.RealEstate.Persistence.Repositories.CrossRepositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Million.API.RealEstate.Persistence.Repositories
{
    public class PropertyTraceRepository : GenericRepository<PropertyTraceEntity>, IPropertyTraceRepository
    {
        public PropertyTraceRepository(IMongoDatabase database) : base(database, "PropertyTraceEntity")
        {
        }

        public async Task<List<PropertyTraceEntity>> GetPropertyTraceByPropertyId(string propertyId)
        {
            // Verificar que el Id no sea nulo o vacío
            if (string.IsNullOrEmpty(propertyId))
                throw new ArgumentException("The property ID must be provided.", nameof(propertyId));

            // Construir el filtro para buscar trazas por IdProperty
            var filter = Builders<PropertyTraceEntity>.Filter.Eq(trace => trace.IdProperty, propertyId);

            // Ejecutar la consulta asincrónicamente
            var propertyTraces = await _collection.Find(filter).ToListAsync();

            return propertyTraces;
        }
    }
}