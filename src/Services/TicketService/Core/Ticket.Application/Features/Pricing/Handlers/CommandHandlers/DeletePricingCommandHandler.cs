using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Pricing.Commands;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Pricing.Handlers.CommandHandlers
{
    public class DeletePricingCommandHandler : IRequestHandler<DeletePricingCommand, BaseResponse>
    {
        private readonly IPricingRepository _pricingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeletePricingCommandHandler> _logger;

        public DeletePricingCommandHandler(IPricingRepository pricingRepository, IUnitOfWork unitOfWork, ILogger<DeletePricingCommandHandler> logger)
        {
            _pricingRepository = pricingRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeletePricingCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _pricingRepository.GetByIdAsync(request.PricingID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Pricing not found"
                    };
                }

                await _pricingRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Pricing deleted successfully",
                    Data = new
                    {
                        PricingID = values.PricingID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the pricing");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
