using AutoMapper;
using Catalog.Application.Features.Movie.Queries;
using Catalog.Application.Features.Movie.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Movie.Handlers.QueryHandlers
{
    public class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, List<GetMoviesQueryResult>>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMoviesQueryHandler> _logger;

        public GetMoviesQueryHandler(IMovieRepository movieRepository, IMapper mapper, ILogger<GetMoviesQueryHandler> logger)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetMoviesQueryResult>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieRepository.ListAsync(cancellationToken);
                return _mapper.Map<List<GetMoviesQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the movies");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
