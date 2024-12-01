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
    public class UpdateOwnerCommandHandler : IRequestHandler<UpdateOwnerCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateOwnerCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdateOwnerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new UpdateOwnerDtoValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request.OwnerDto, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to Send Owner: {firstError}");
                }

                // Obtener el Owner existente
                var existingOwner = await _unitOfWork.Repository<OwnerEntity>().GetAsync(request.OwnerDto.Id);

                if (existingOwner == null)
                {
                    response.Success = false;
                    response.Message = "Owner not found";
                    return response;
                }

                // Actualizar los valores
                existingOwner.Name = request.OwnerDto.Name;
                existingOwner.Address = request.OwnerDto.Address;
                existingOwner.Photo = request.OwnerDto.Photo;
                existingOwner.BirthDay = request.OwnerDto.BirthDay;

                // Guardar los cambios
                await _unitOfWork.Repository<OwnerEntity>().UpdateAsync(request.OwnerDto.Id, existingOwner);

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Owner updated successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "UpdatedOwner",
                        Value = JsonConvert.SerializeObject(existingOwner)
                    }
                };
            }
            catch (Exception ex)
            {
                // Manejar errores
                response.Success = false;
                response.Message = "Error updating Owner";
                response.Errors = new List<string> { ex.Message };
            }
            return response;
        }
    }
}