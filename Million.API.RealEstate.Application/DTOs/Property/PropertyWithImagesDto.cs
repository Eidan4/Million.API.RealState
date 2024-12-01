using System.Collections.Generic;
using Million.API.RealEstate.Application.DTOs.PropertyImage;

namespace Million.API.RealEstate.Application.DTOs.Property
{
    public class PropertyWithImagesDto
    {
        public PropertyDto Property { get; set; }
        public List<PropertyImageDto> Images { get; set; }
    }
}