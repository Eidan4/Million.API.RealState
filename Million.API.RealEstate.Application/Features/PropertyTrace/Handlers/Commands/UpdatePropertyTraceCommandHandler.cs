using AutoMapper;
using MediatR;
using Million.API.RealEstate.Application.Contracts.Persistence.CrossRepositories;
using Million.API.RealEstate.Application.DTOs.PropertyTrace;
using Million.API.RealEstate.Application.Features.PropertyTrace.Requests.Commands;
using Million.API.RealEstate.Application.Response;
using Million.API.RealEstate.Domain.PropertyTrace;

namespace Million.API.RealEstate.Application.Features.PropertyTrace.Handlers.Commands
{
    public class UpdatePropertyTraceCommandHandler : IRequestHandler<UpdatePropertyTraceCommand, BaseCommandResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdatePropertyTraceCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BaseCommandResponse> Handle(UpdatePropertyTraceCommand request, CancellationToken cancellationToken)
        {
            var response = new BaseCommandResponse();

            try
            {
                var validator = new UpdatePropertyTraceDtoValidator();
                var validationResult = await validator.ValidateAsync(request.PropertyTraceDto, cancellationToken);

                if (!validationResult.IsValid)
                {
                    response.Success = false;
                    response.Message = "Validation errors";
                    response.Errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                    return response;
                }

                var existingPropertyTrace = await _unitOfWork.Repository<PropertyTraceEntity>().GetAsync(request.PropertyTraceDto.Id);

                if (existingPropertyTrace == null)
                {
                    response.Success = false;
                    response.Message = "Property Trace not found";
                    return response;
                }

                _mapper.Map(request.PropertyTraceDto, existingPropertyTrace);
                await _unitOfWork.Repository<PropertyTraceEntity>().UpdateAsync(request.PropertyTraceDto.Id, existingPropertyTrace);

                response.Success = true;
                response.Message = "Property Trace updated successfully";
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = "Error updating Property Trace";
                response.Errors = new List<string> { ex.Message };
            }

            return response;
        }
    }
}
