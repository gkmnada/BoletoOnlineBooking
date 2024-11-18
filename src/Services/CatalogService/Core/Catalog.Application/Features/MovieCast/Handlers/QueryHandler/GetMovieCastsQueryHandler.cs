using AutoMapper;
using Catalog.Application.Features.MovieCast.Queries;
using Catalog.Application.Features.MovieCast.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCast.Handlers.QueryHandler
{
    public class GetMovieCastsQueryHandler : IRequestHandler<GetMovieCastsQuery, List<GetMovieCastsQueryResult>>
    {
        private readonly IMovieCastRepository _movieCastRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieCastsQueryHandler> _logger;

        public GetMovieCastsQueryHandler(IMovieCastRepository movieCastRepository, IMapper mapper, ILogger<GetMovieCastsQueryHandler> logger)
        {
            _movieCastRepository = movieCastRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetMovieCastsQueryResult>> Handle(GetMovieCastsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieCastRepository.ListByFilterAsync(x => x.MovieID == request.MovieID, cancellationToken);
                return _mapper.Map<List<GetMovieCastsQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the movie casts");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
