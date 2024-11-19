using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Features.Category.Queries;
using Ticket.Application.Features.Category.Results;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Category.Handlers.QueryHandlers
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, GetCategoryByIdQueryResult>
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

        public GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository, IMapper mapper, ILogger<GetCategoryByIdQueryHandler> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetCategoryByIdQueryResult> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _categoryRepository.GetByIdAsync(request.category_id, cancellationToken);
                return _mapper.Map<GetCategoryByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the category by id");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
