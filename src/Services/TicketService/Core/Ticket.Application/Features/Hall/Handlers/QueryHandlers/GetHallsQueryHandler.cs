using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Hall.Queries;
using Ticket.Application.Features.Hall.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Hall.Handlers.QueryHandlers
{
    public class GetHallsQueryHandler : IRequestHandler<GetHallsQuery, List<GetHallsQueryResult>>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetHallsQueryHandler> _logger;

        public GetHallsQueryHandler(IHallRepository hallRepository, IMapper mapper, ILogger<GetHallsQueryHandler> logger)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetHallsQueryResult>> Handle(GetHallsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _hallRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetHallsQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching halls");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
