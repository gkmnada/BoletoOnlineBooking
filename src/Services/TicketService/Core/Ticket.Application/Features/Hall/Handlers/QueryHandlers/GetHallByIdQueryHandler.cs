using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Hall.Queries;
using Ticket.Application.Features.Hall.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Hall.Handlers.QueryHandlers
{
    public class GetHallByIdQueryHandler : IRequestHandler<GetHallByIdQuery, GetHallByIdQueryResult>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetHallByIdQueryHandler> _logger;

        public GetHallByIdQueryHandler(IHallRepository hallRepository, IMapper mapper, ILogger<GetHallByIdQueryHandler> logger)
        {
            _hallRepository = hallRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetHallByIdQueryResult> Handle(GetHallByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _hallRepository.GetByIdAsync(request.hall_id, cancellationToken);
                return _mapper.Map<GetHallByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching hall by id");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
