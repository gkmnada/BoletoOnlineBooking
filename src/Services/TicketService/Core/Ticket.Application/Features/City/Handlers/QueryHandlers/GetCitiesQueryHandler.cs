using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.City.Queries;
using Ticket.Application.Features.City.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.City.Handlers.QueryHandlers
{
    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<GetCitiesQueryResult>>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCitiesQueryHandler> _logger;

        public GetCitiesQueryHandler(ICityRepository cityRepository, IMapper mapper, ILogger<GetCitiesQueryHandler> logger)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetCitiesQueryResult>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cityRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetCitiesQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching cities");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
