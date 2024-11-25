using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Seat.Commands;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Seat.Handlers.CommandHandlers
{
    public class DeleteSeatCommandHandler : IRequestHandler<DeleteSeatCommand, BaseResponse>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteSeatCommandHandler> _logger;

        public DeleteSeatCommandHandler(ISeatRepository seatRepository, IUnitOfWork unitOfWork, ILogger<DeleteSeatCommandHandler> logger)
        {
            _seatRepository = seatRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteSeatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _seatRepository.GetByIdAsync(request.SeatID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Seat not found",
                    };
                }

                await _seatRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Seat deleted successfully",
                    Data = new
                    {
                        SeatID = values.SeatID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the seat");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
