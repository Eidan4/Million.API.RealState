using MediatR;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries
{
    public class GetPropertyTraceByIdQuery : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}