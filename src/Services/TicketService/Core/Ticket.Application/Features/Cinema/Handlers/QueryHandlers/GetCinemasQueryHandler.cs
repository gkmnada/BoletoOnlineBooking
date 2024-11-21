using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Cinema.Queries;
using Ticket.Application.Features.Cinema.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Cinema.Handlers.QueryHandlers
{
    public class GetCinemasQueryHandler : IRequestHandler<GetCinemasQuery, List<GetCinemasQueryResult>>
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCinemasQueryHandler> _logger;

        public GetCinemasQueryHandler(ICinemaRepository cinemaRepository, IMapper mapper, ILogger<GetCinemasQueryHandler> logger)
        {
            _cinemaRepository = cinemaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetCinemasQueryResult>> Handle(GetCinemasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cinemaRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetCinemasQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching cinemas");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
