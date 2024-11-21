using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Pricing.Queries;
using Ticket.Application.Features.Pricing.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Pricing.Handlers.QueryHandlers
{
    public class GetPricingsQueryHandler : IRequestHandler<GetPricingsQuery, List<GetPricingsQueryResult>>
    {
        private readonly IPricingRepository _pricingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetPricingsQueryHandler> _logger;

        public GetPricingsQueryHandler(IPricingRepository pricingRepository, IMapper mapper, ILogger<GetPricingsQueryHandler> logger)
        {
            _pricingRepository = pricingRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetPricingsQueryResult>> Handle(GetPricingsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _pricingRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetPricingsQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the pricings");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
