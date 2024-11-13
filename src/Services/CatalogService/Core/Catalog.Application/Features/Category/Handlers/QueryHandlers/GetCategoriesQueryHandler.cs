using AutoMapper;
using Catalog.Application.Features.Category.Queries;
using Catalog.Application.Features.Category.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Category.Handlers.QueryHandlers
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
                _logger.LogError(ex, "An error occurred while fetching the categories");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
