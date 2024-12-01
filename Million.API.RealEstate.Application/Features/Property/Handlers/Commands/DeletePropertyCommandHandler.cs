using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.Features.Property.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Property;
using Newtonsoft.Json;
using FluentValidation.Results;

namespace Million.API.RealEstate.Application.Features.Property.Handlers.Commands
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new DeletePropertyCommandValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors occurred";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                // Intentar obtener la propiedad
                var property = await _unitOfWork.Repository<PropertyEntity>().GetAsync(request.Id);

                if (property == null)
                {
                    response.Success = false;
                    response.Message = "Property not found";
                    return response;
                }

                // Eliminar la propiedad
                await _unitOfWork.Repository<PropertyEntity>().DeleteAsync(request.Id);

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Property deleted successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "DeletedProperty",
                        Value = JsonConvert.SerializeObject(property)
                    }
                };
            }
            catch (Exception ex)
            {
                // Manejar errores
                response.Success = false;
                response.Message = "Error deleting Property";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}