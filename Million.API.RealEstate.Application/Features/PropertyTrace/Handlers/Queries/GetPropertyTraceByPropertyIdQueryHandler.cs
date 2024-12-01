using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Queries;
using Million.API.RealEstate.Application.Response;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Queries
{
    public class GetPropertyTraceByPropertyIdQueryHandler : IRequestHandler<GetPropertyTraceByPropetyIdQuery, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetPropertyTraceByPropertyIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(GetPropertyTraceByPropetyIdQuery request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Obtener trazas de la propiedad utilizando el repositorio
                var propertyTraces = await _unitOfWork.PropertyTraceRepository.GetPropertyTraceByPropertyId(request.Id);

                if (propertyTraces == null || !propertyTraces.Any())
                {
                    response.Success = false;
                    response.Message = "No Property Traces found for the given Property ID";
                    return response;
                }

                // Mapear las trazas al DTO
                var propertyTraceDtos = _mapper.Map<List<PropertyTraceDto>>(propertyTraces);

                response.Success = true;
                response.Message = "Property Traces retrieved successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyTraces",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(propertyTraceDtos)
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
