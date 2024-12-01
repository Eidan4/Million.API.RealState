using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.PropertyImage;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyTrace;
using FluentValidation.Results;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Commands
{
    public class DeletePropertyTraceCommandHandler : IRequestHandler<DeletePropertyTraceCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyTraceCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BaseCommandResponse> Handle(DeletePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new DeletePropertyTraceCommandValidator();
                ValidationResult validationResult = await validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors occurred";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                var propertyTrace = await _unitOfWork.Repository<PropertyTraceEntity>().GetAsync(request.Id);

                if (propertyTrace == null)
                {
                    response.Success = false;
                    response.Message = "Property Trace not found";
                    return response;
                }

                await _unitOfWork.Repository<PropertyTraceEntity>().DeleteAsync(request.Id);

                response.Success = true;
                response.Message = "Property Trace deleted successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error deleting Property Trace";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
