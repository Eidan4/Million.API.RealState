using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries
{
    public class GetAllPropertyTracesQuery : IRequest<BaseCommandResponse> { }
}