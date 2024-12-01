using MongoDB.Driver;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Domain.PropertyImage;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Domain.Property;
using Million.API.RealEstate.Persistence.Repositories.CrossRepositories;

namespace Million.API.RealEstate.Persistence.Repositories
{
    public class PropertyRepository : GenericRepository<PropertyEntity>, IPropertyRepository
    {
        private readonly IMongoCollection<PropertyImageEntity> _propertyImageCollection;

        public PropertyRepository(IMongoDatabase database) : base(database, "PropertyEntity")
        {
            _propertyImageCollection = database.GetCollection<PropertyImageEntity>("PropertyImageEntity");
        }

        public async Task<List<PropertyWithImagesDto>> GetPropertiesWithImagesAsync(PropertyFilterDto filters)
        {
            var filterBuilder = Builders<PropertyEntity>.Filter;
            var filtersList = new List<FilterDefinition<PropertyEntity>>();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(filters.Name))
                filtersList.Add(filterBuilder.Regex(p => p.Name, new MongoDB.Bson.BsonRegularExpression(filters.Name, "i")));

            if (!string.IsNullOrEmpty(filters.Address))
                filtersList.Add(filterBuilder.Regex(p => p.Address, new MongoDB.Bson.BsonRegularExpression(filters.Address, "i")));

            if (filters.MinPrice.HasValue)
                filtersList.Add(filterBuilder.Gte(p => p.Price, (double)filters.MinPrice.Value));

            if (filters.MaxPrice.HasValue)
                filtersList.Add(filterBuilder.Lte(p => p.Price, (double)filters.MaxPrice.Value));

            var finalFilter = filtersList.Count > 0 ? filterBuilder.And(filtersList) : filterBuilder.Empty;

            // Obtener propiedades filtradas
            var properties = await _collection.Find(finalFilter).ToListAsync();

            // Obtener imágenes relacionadas
            var propertyImages = await _propertyImageCollection.Find(_ => true).ToListAsync();

            // Combinar propiedades con imágenes
            var propertiesWithImages = properties.Select(property => new PropertyWithImagesDto
            {
                Property = new PropertyDto
                {
                    Id = property.Id,
                    Name = property.Name,
                    Address = property.Address,
                    Price = (decimal)property.Price,
                    CodeInternal = property.CodeInternal,
                    Year = property.Year,
                    IdOwner = property.IdOwner
                },
                Images = propertyImages
                    .Where(image => image.IdProperty == property.Id)
                    .Select(image => new PropertyImageDto
                    {
                        Id = image.Id,
                        IdProperty = image.IdProperty,
                        File = image.File,
                        Enabled = image.Enabled
                    }).ToList()
            }).ToList();

            return propertiesWithImages;
        }

        public async Task<PropertyWithImagesDto?> GetPropertyWithImagesByIdAsync(string id)
        {
            // Verificar que el Id no sea nulo o vacío
            if (string.IsNullOrEmpty(id))
                throw new ArgumentException("The property ID must be provided.", nameof(id));

            // Buscar la propiedad por Id
            var property = await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();

            if (property == null)
                return null; // Si no se encuentra la propiedad, retorna null

            // Obtener imágenes relacionadas
            var propertyImages = await _propertyImageCollection.Find(img => img.IdProperty == id).ToListAsync();

            // Combinar propiedad con imágenes
            var propertyWithImages = new PropertyWithImagesDto
            {
                Property = new PropertyDto
                {
                    Id = property.Id,
                    Name = property.Name,
                    Address = property.Address,
                    Price = (decimal)property.Price,
                    CodeInternal = property.CodeInternal,
                    Year = property.Year,
                    IdOwner = property.IdOwner
                },
                Images = propertyImages.Select(image => new PropertyImageDto
                {
                    Id = image.Id,
                    IdProperty = image.IdProperty,
                    File = image.File,
                    Enabled = image.Enabled
                }).ToList()
            };

            return propertyWithImages;
        }
    }
}