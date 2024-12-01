using MediatR;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Property.Requests.Queries
{
    public class GetPropertiesWithImagesByIdQuery : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}