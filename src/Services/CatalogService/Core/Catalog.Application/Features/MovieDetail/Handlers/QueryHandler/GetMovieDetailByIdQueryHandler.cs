using AutoMapper;
using Catalog.Application.Features.MovieDetail.Queries;
using Catalog.Application.Features.MovieDetail.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieDetail.Handlers.QueryHandler
{
    public class GetMovieDetailByIdQueryHandler : IRequestHandler<GetMovieDetailByIdQuery, GetMovieDetailByIdQueryResult>
    {
        private readonly IMovieDetailRepository _movieDetailRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieDetailByIdQueryHandler> _logger;

        public GetMovieDetailByIdQueryHandler(IMovieDetailRepository movieDetailRepository, IMapper mapper, ILogger<GetMovieDetailByIdQueryHandler> logger)
        {
            _movieDetailRepository = movieDetailRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetMovieDetailByIdQueryResult> Handle(GetMovieDetailByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieDetailRepository.GetByIdAsync(request.DetailID, cancellationToken);
                return _mapper.Map<GetMovieDetailByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the movie detail by ID");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
