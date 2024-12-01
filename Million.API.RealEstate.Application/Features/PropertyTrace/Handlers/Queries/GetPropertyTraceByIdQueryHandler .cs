using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyTrace;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Queries
{
    public class GetPropertyTraceByIdQueryHandler : IRequestHandler<GetPropertyTraceByIdQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPropertyTraceByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetPropertyTraceByIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var propertyTrace = await _unitOfWork.Repository<PropertyTraceEntity>().GetAsync(request.Id);

                if (propertyTrace == null)
                {
                    response.Success = false;
                    response.Message = "Property Trace not found";
                    return response;
                }

                response.Success = true;
                response.Message = "Property Trace retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyTraceDetails",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(propertyTrace)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error retrieving Property Trace";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
