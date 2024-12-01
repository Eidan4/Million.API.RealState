using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Property.Requests.Queries
{
    public class GetAllPropertiesQuery : IRequest<BaseCommandResponse> { }
}