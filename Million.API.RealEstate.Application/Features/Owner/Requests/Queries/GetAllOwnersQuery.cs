using MediatR;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.Owner.Requests.Queries
{
    public class GetAllOwnersQuery : IRequest<BaseCommandResponse>
    {
    }
}
