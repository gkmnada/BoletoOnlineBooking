using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Hall.Commands;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Hall.Handlers.CommandHandlers
{
    public class DeleteHallCommandHandler : IRequestHandler<DeleteHallCommand, BaseResponse>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteHallCommandHandler> _logger;

        public DeleteHallCommandHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork, ILogger<DeleteHallCommandHandler> logger)
        {
            _hallRepository = hallRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteHallCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _hallRepository.GetByIdAsync(request.hall_id, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Hall not found"
                    };
                }

                await _hallRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Hall deleted successfully",
                    Data = new
                    {
                        id = values.id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting hall");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
