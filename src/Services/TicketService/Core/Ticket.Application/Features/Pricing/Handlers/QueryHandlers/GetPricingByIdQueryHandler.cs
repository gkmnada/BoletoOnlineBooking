using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Pricing.Queries;
using Ticket.Application.Features.Pricing.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Pricing.Handlers.QueryHandlers
{
    public class GetPricingByIdQueryHandler : IRequestHandler<GetPricingByIdQuery, GetPricingByIdQueryResult>
    {
        private readonly IPricingRepository _pricingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPricingByIdQueryHandler> _logger;

        public GetPricingByIdQueryHandler(IPricingRepository pricingRepository, IMapper mapper, ILogger<GetPricingByIdQueryHandler> logger)
        {
            _pricingRepository = pricingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetPricingByIdQueryResult> Handle(GetPricingByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _pricingRepository.GetByIdAsync(request.PricingID, cancellationToken);
                return _mapper.Map<GetPricingByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the pricing by ID");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
