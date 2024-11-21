using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.City.Queries;
using Ticket.Application.Features.City.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.City.Handlers.QueryHandlers
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, GetCityByIdQueryResult>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCityByIdQueryHandler> _logger;

        public GetCityByIdQueryHandler(ICityRepository cityRepository, IMapper mapper, ILogger<GetCityByIdQueryHandler> logger)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCityByIdQueryResult> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cityRepository.GetByIdAsync(request.city_id, cancellationToken);
                return _mapper.Map<GetCityByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting city by id");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
