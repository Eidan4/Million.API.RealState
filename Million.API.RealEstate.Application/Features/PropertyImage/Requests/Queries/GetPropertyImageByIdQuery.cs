using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Requests.Queries
{
    public class GetPropertyImageByIdQuery : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}