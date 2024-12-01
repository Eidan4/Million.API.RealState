using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Owner;
using Newtonsoft.Json;
using System.Linq;

namespace Million.API.RealEstate.Application.Features.Owner.Handlers.Commands
{
    public class CreateOwnerCommandHandler : IRequestHandler<OwnerCommand, BaseCommandResponse>
    {
        #region Dependencies
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        #endregion

        public CreateOwnerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(OwnerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();
            try
            {
                // Crear validador y validar el DTO
                var validator = new OwnerDtoValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request.OwnerDto, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to Send Owner: {firstError}");
                }

                // Mapear OwnerDto a OwnerEntity
                var owner = _mapper.Map<OwnerEntity>(request.OwnerDto);

                // Guardar en la base de datos usando UnitOfWork
                await _unitOfWork.Repository<OwnerEntity>().AddAsync(owner);

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Owner created successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "OwnerCreate",
                        Value = JsonConvert.SerializeObject(owner)
                    }
                };
            }
            catch (Exception ex)
            {
                // Configurar respuesta en caso de error
                response.Success = false;
                response.Message = "Error al crear Owner";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}