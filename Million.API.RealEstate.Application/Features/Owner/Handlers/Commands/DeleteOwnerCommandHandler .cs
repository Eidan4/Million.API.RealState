using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Common;
using Million.API.RealEstate.Application.DTOs.Owner;
using Million.API.RealEstate.Application.Features.Owner.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.Owner;
using Newtonsoft.Json;
using FluentValidation.Results;

namespace Million.API.RealEstate.Application.Features.Owner.Handlers.Commands
{
    public class DeleteOwnerCommandHandler : IRequestHandler<DeleteOwnerCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteOwnerCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(DeleteOwnerCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new DeleteOwnerCommandValidator();
                ValidationResult validatorResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validatorResult.IsValid)
                {
                    var firstError = validatorResult.Errors.FirstOrDefault()?.ErrorMessage;
                    throw new Exception($"Failed to Send Owner: {firstError}");
                }

                // Intentar obtener el Owner
                var owner = await _unitOfWork.Repository<OwnerEntity>().GetAsync(request.Id);

                if (owner == null)
                {
                    response.Success = false;
                    response.Message = "Owner not found";
                    return response;
                }

                // Eliminar el Owner
                await _unitOfWork.Repository<OwnerEntity>().DeleteAsync(request.Id);

                // Configurar respuesta exitosa
                response.Success = true;
                response.Message = "Owner deleted successfully";
                response.Parameters = new List<ParameterDto>
                {
                    new ParameterDto
                    {
                        Name = "DeletedOwner",
                        Value = JsonConvert.SerializeObject(owner)
                    }
                };
            }
            catch (Exception ex)
            {
                // Manejar errores
                response.Success = false;
                response.Message = "Error deleting Owner";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
