using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Cinema.Commands;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Cinema.Handlers.CommandHandlers
{
    public class DeleteCinemaCommandHandler : IRequestHandler<DeleteCinemaCommand, BaseResponse>
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCinemaCommandHandler> _logger;

        public DeleteCinemaCommandHandler(ICinemaRepository cinemaRepository, IUnitOfWork unitOfWork, ILogger<DeleteCinemaCommandHandler> logger)
        {
            _cinemaRepository = cinemaRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteCinemaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cinemaRepository.GetByIdAsync(request.cinema_id, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Cinema not found"
                    };
                }

                await _cinemaRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Cinema deleted successfully",
                    Data = new
                    {
                        id = values.id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting cinema");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
