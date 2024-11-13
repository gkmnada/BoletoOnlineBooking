using AutoMapper;
using Catalog.Application.Features.Movie.Queries;
using Catalog.Application.Features.Movie.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Movie.Handlers.QueryHandlers
{
    public class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, GetMovieByIdQueryResult>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieByIdQueryHandler> _logger;

        public GetMovieByIdQueryHandler(IMovieRepository movieRepository, IMapper mapper, ILogger<GetMovieByIdQueryHandler> logger)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetMovieByIdQueryResult> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieRepository.GetByIdAsync(request.MovieID, cancellationToken);
                return _mapper.Map<GetMovieByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the movie by ID");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
