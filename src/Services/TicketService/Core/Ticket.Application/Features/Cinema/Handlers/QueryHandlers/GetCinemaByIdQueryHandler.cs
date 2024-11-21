using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Cinema.Queries;
using Ticket.Application.Features.Cinema.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Cinema.Handlers.QueryHandlers
{
    public class GetCinemaByIdQueryHandler : IRequestHandler<GetCinemaByIdQuery, GetCinemaByIdQueryResult>
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCinemaByIdQueryHandler> _logger;

        public GetCinemaByIdQueryHandler(ICinemaRepository cinemaRepository, IMapper mapper, ILogger<GetCinemaByIdQueryHandler> logger)
        {
            _cinemaRepository = cinemaRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCinemaByIdQueryResult> Handle(GetCinemaByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cinemaRepository.GetByIdAsync(request.cinema_id, cancellationToken);
                return _mapper.Map<GetCinemaByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching cinema by id");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
