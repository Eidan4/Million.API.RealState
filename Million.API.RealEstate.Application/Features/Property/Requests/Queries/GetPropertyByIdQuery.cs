using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Property.Requests.Queries
{
    public class GetPropertyByIdQuery : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}