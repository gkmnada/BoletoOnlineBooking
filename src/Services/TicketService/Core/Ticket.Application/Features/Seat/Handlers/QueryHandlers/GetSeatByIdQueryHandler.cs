using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Seat.Queries;
using Ticket.Application.Features.Seat.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Seat.Handlers.QueryHandlers
{
    public class GetSeatByIdQueryHandler : IRequestHandler<GetSeatByIdQuery, GetSeatByIdQueryResult>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSeatByIdQueryHandler> _logger;

        public GetSeatByIdQueryHandler(ISeatRepository seatRepository, IMapper mapper, ILogger<GetSeatByIdQueryHandler> logger)
        {
            _seatRepository = seatRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSeatByIdQueryResult> Handle(GetSeatByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _seatRepository.GetByIdAsync(request.seat_id, cancellationToken);
                return _mapper.Map<GetSeatByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the seat by id");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
