using AutoMapper;
using Catalog.Application.Features.MovieCrew.Queries;
using Catalog.Application.Features.MovieCrew.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCrew.Handlers.QueryHandler
{
    public class GetMovieCrewsQueryHandler : IRequestHandler<GetMovieCrewsQuery, List<GetMovieCrewsQueryResult>>
    {
        private readonly IMovieCrewRepository _movieCrewRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieCrewsQueryHandler> _logger;

        public GetMovieCrewsQueryHandler(IMovieCrewRepository movieCrewRepository, IMapper mapper, ILogger<GetMovieCrewsQueryHandler> logger)
        {
            _movieCrewRepository = movieCrewRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetMovieCrewsQueryResult>> Handle(GetMovieCrewsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieCrewRepository.ListByFilterAsync(x => x.MovieID == request.MovieID, cancellationToken);
                return _mapper.Map<List<GetMovieCrewsQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the movie crews");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
