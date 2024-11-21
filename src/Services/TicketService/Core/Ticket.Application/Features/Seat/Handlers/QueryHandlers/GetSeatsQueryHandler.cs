using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Seat.Queries;
using Ticket.Application.Features.Seat.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Seat.Handlers.QueryHandlers
{
    public class GetSeatsQueryHandler : IRequestHandler<GetSeatsQuery, List<GetSeatsQueryResult>>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSeatsQueryHandler> _logger;

        public GetSeatsQueryHandler(ISeatRepository seatRepository, IMapper mapper, ILogger<GetSeatsQueryHandler> logger)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetSeatsQueryResult>> Handle(GetSeatsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _seatRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetSeatsQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the seats");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
