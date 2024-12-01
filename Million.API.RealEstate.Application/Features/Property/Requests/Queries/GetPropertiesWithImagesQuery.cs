using MediatR;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Property.Requests.Queries
{
    public class GetPropertiesWithImagesQuery : IRequest<BaseCommandResponse>
    {
        public PropertyFilterDto Filters { get; set; }
    }
}