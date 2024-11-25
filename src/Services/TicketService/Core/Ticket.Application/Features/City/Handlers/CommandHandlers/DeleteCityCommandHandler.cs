using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.City.Commands;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.City.Handlers.CommandHandlers
{
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, BaseResponse>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCityCommandHandler> _logger;

        public DeleteCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, ILogger<DeleteCityCommandHandler> logger)
        {
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cityRepository.GetByIdAsync(request.CityID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "City not found"
                    };
                }

                await _cityRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "City deleted successfully",
                    Data = new
                    {
                        CityID = values.CityID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting city");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
