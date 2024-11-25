using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Session.Commands;
using Ticket.Application.Features.Session.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Session.Handlers.CommandHandlers
{
    public class UpdateSessionCommandHandler : IRequestHandler<UpdateSessionCommand, BaseResponse>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSessionCommandHandler> _logger;
        private readonly UpdateSessionValidator _validator;

        public UpdateSessionCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateSessionCommandHandler> logger, UpdateSessionValidator validator)
        {
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateSessionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _sessionRepository.GetByIdAsync(request.SessionID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Session not found",
                    };
                }

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Validation failed",
                        Errors = errors
                    };
                }

                _mapper.Map(request, values);

                await _sessionRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Session updated successfully",
                    Data = new
                    {
                        SessionID = values.SessionID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the session");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
