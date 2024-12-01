using AutoMapper;
using FluentValidation;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyTrace;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Commands
{
    public class CreatePropertyTraceCommandHandler : IRequestHandler<CreatePropertyTraceCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreatePropertyTraceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreatePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                // Validar el DTO
                var validator = new PropertyTraceDtoValidator();
                var validationResult = await validator.ValidateAsync(request.PropertyTraceDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                // Mapear el DTO a la entidad
                var propertyTrace = _mapper.Map<PropertyTraceEntity>(request.PropertyTraceDto);

                // Guardar en la base de datos
                await _unitOfWork.Repository<PropertyTraceEntity>().AddAsync(propertyTrace);

                response.Success = true;
                response.Message = "Property Trace created successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "CreatedPropertyTrace",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(propertyTrace)
                    }
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error creating Property Trace";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}