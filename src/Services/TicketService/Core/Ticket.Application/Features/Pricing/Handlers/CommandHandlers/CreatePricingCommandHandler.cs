using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Pricing.Commands;
using Ticket.Application.Features.Pricing.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Pricing.Handlers.CommandHandlers
{
    public class CreatePricingCommandHandler : IRequestHandler<CreatePricingCommand, BaseResponse>
    {
        private readonly IPricingRepository _pricingRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreatePricingCommandHandler> _logger;
        private readonly CreatePricingValidator _validator;

        public CreatePricingCommandHandler(IPricingRepository pricingRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreatePricingCommandHandler> logger, CreatePricingValidator validator)
        {
            _pricingRepository = pricingRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreatePricingCommand request, CancellationToken cancellationToken)
        {
            try
            {
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

                var entity = _mapper.Map<Domain.Entities.Pricing>(request);

                await _pricingRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Pricing created successfully",
                    Data = new
                    {
                        id = entity.id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the pricing");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
