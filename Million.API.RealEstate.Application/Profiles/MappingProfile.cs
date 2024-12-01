using AutoMapper;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Domain.Owner;
using Million.API.RealEstate.Domain.Property;
using Million.API.RealEstate.Domain.PropertyImage;
using Million.API.RealEstate.Domain.PropertyTrace;

namespace Million.API.RealEstate.Application.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Owner mappings
            CreateMap<OwnerDto, OwnerEntity>().ReverseMap();

            // Property mappings
            CreateMap<PropertyDto, PropertyEntity>().ReverseMap();

            // PropertyImage mappings
            CreateMap<PropertyImageDto, PropertyImageEntity>().ReverseMap();

            // PropertyTrace mappings
            CreateMap<PropertyTraceDto, PropertyTraceEntity>().ReverseMap();
        }
    }
}
