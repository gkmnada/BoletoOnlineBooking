using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Pricing.Commands;
using Ticket.Application.Features.Pricing.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Pricing.Handlers.CommandHandlers
{
    public class UpdatePricingCommandHandler : IRequestHandler<UpdatePricingCommand, BaseResponse>
    {
        private readonly IPricingRepository _pricingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdatePricingCommandHandler> _logger;
        private readonly UpdatePricingValidator _validator;

        public UpdatePricingCommandHandler(IPricingRepository pricingRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdatePricingCommandHandler> logger, UpdatePricingValidator validator)
        {
            _pricingRepository = pricingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdatePricingCommand request, CancellationToken cancellationToken)
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

                await _pricingRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Pricing updated successfully",
                    Data = new
                    {
                        PricingID = values.PricingID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the pricing");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
