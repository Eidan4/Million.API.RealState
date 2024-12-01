using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Features.Property.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Property;
using Newtonsoft.Json;
using System.Linq;

namespace Million.API.RealEstate.Application.Features.Property.Handlers.Commands
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, BaseCommandResponse>
    {
        #region Dependencies
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        public CreatePropertyCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                // Crear validador y validar el DTO
                var validator = new PropertyDtoValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request.PropertyDto, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to create Property: {firstError}");
                }

                // Mapear PropertyDto a PropertyEntity
                var property = _mapper.Map<PropertyEntity>(request.PropertyDto);

                // Guardar en la base de datos usando UnitOfWork
                await _unitOfWork.Repository<PropertyEntity>().AddAsync(property);


                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Property created successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "PropertyCreated",
                        Value = JsonConvert.SerializeObject(property)
                    }
                };
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error creating Property";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
