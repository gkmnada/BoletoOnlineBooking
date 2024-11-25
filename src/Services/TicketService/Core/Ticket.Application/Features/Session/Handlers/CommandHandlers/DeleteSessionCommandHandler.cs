using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Session.Commands;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Session.Handlers.CommandHandlers
{
    public class DeleteSessionCommandHandler : IRequestHandler<DeleteSessionCommand, BaseResponse>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteSessionCommandHandler> _logger;

        public DeleteSessionCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork, ILogger<DeleteSessionCommandHandler> logger)
        {
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteSessionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _sessionRepository.GetByIdAsync(request.SessionID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Session not found"
                    };
                }

                await _sessionRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Session deleted successfully",
                    Data = new
                    {
                        SessionID = values.SessionID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the session");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
