using AutoMapper;
using Catalog.Application.Features.MovieCrew.Queries;
using Catalog.Application.Features.MovieCrew.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCrew.Handlers.QueryHandler
{
    public class GetMovieCrewByIdQueryHandler : IRequestHandler<GetMovieCrewByIdQuery, GetMovieCrewByIdQueryResult>
    {
        private readonly IMovieCrewRepository _movieCrewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieCrewByIdQueryHandler> _logger;

        public GetMovieCrewByIdQueryHandler(IMovieCrewRepository movieCrewRepository, IMapper mapper, ILogger<GetMovieCrewByIdQueryHandler> logger)
        {
            _movieCrewRepository = movieCrewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetMovieCrewByIdQueryResult> Handle(GetMovieCrewByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieCrewRepository.GetByIdAsync(request.CrewID, cancellationToken);
                return _mapper.Map<GetMovieCrewByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the movie crew by ID");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
