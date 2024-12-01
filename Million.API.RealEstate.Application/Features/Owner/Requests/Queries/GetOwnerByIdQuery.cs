using MediatR;
using Million.API.RealEstate.Application.Response;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Million.API.RealEstate.Application.Features.Owner.Requests.Queries
{
    public class GetOwnerByIdQuery : IRequest<BaseCommandResponse>
    {
        public string Id { get; set; }
    }
}