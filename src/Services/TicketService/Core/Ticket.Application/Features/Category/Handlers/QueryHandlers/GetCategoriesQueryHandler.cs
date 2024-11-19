using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Category.Queries;
using Ticket.Application.Features.Category.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Category.Handlers.QueryHandlers
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, List<GetCategoriesQueryResult>>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoriesQueryHandler> _logger;

        public GetCategoriesQueryHandler(ICategoryRepository categoryRepository, IMapper mapper, ILogger<GetCategoriesQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetCategoriesQueryResult>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _categoryRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetCategoriesQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
