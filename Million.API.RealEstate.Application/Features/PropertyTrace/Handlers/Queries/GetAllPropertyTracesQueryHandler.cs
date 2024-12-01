using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyTrace;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Queries
{
    public class GetAllPropertyTracesQueryHandler : IRequestHandler<GetAllPropertyTracesQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetAllPropertyTracesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(GetAllPropertyTracesQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var propertyTraces = await _unitOfWork.Repository<PropertyTraceEntity>().GetAllAsync();

                response.Success = true;
                response.Message = "Property Traces retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyTraceList",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(propertyTraces)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving Property Traces";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
