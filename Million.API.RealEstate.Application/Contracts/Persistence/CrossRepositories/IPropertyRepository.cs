using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Domain.Property;

namespace Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories
{
    public interface IPropertyRepository : IGenericRepository<PropertyEntity>
    {
        Task<List<PropertyWithImagesDto>> GetPropertiesWithImagesAsync(PropertyFilterDto filters);
        Task<PropertyWithImagesDto?> GetPropertyWithImagesByIdAsync(string id);
    }
}