using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.Property;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Application.Features.PropertyImage.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyImage;
using FluentValidation.Results;

namespace Million.API.RealEstate.Application.Features.PropertyImage.Handlers.Commands
{
    public class DeletePropertyImageCommandHandler : IRequestHandler<DeletePropertyImageCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyImageCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(DeletePropertyImageCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new DeletePropertyImageCommandValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors occurred";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                var propertyImage = await _unitOfWork.Repository<PropertyImageEntity>().GetAsync(request.Id);

                if (propertyImage == null)
                {
                    response.Success = false;
                    response.Message = "Property Image not found";
                    return response;
                }

                await _unitOfWork.Repository<PropertyImageEntity>().DeleteAsync(request.Id);

                response.Success = true;
                response.Message = "Property Image deleted successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error deleting Property Image";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}