using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Session.Queries;
using Ticket.Application.Features.Session.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Session.Handlers.QueryHandlers
{
    public class GetSessionsQueryHandler : IRequestHandler<GetSessionsQuery, List<GetSessionsQueryResult>>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSessionsQueryHandler> _logger;

        public GetSessionsQueryHandler(ISessionRepository sessionRepository, IMapper mapper, ILogger<GetSessionsQueryHandler> logger)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetSessionsQueryResult>> Handle(GetSessionsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _sessionRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetSessionsQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching sessions");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
