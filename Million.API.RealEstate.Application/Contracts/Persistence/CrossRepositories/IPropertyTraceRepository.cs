using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Domain.Property;
using Million.API.RealEstate.Domain.PropertyTrace;

namespace Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories
{
    public interface IPropertyTraceRepository : IGenericRepository<PropertyTraceEntity>
    {
       Task<List<PropertyTraceEntity>> GetPropertyTraceByPropertyId(string propertyId);

    }
}