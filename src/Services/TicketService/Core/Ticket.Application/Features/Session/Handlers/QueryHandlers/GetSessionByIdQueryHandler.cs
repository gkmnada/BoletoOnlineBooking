using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Session.Queries;
using Ticket.Application.Features.Session.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Session.Handlers.QueryHandlers
{
    public class GetSessionByIdQueryHandler : IRequestHandler<GetSessionByIdQuery, GetSessionByIdQueryResult>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetSessionByIdQueryHandler> _logger;

        public GetSessionByIdQueryHandler(ISessionRepository sessionRepository, IMapper mapper, ILogger<GetSessionByIdQueryHandler> logger)
        {
            _sessionRepository = sessionRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetSessionByIdQueryResult> Handle(GetSessionByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _sessionRepository.GetByIdAsync(request.SessionID, cancellationToken);
                return _mapper.Map<GetSessionByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching session by ID");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
