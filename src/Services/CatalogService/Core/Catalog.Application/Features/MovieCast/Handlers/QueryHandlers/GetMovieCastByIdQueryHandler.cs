using AutoMapper;
using Catalog.Application.Features.MovieCast.Queries;
using Catalog.Application.Features.MovieCast.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCast.Handlers.QueryHandlers
{
    public class GetMovieCastByIdQueryHandler : IRequestHandler<GetMovieCastByIdQuery, GetMovieCastByIdQueryResult>
    {
        private readonly IMovieCastRepository _movieCastRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieCastByIdQueryHandler> _logger;

        public GetMovieCastByIdQueryHandler(IMovieCastRepository movieCastRepository, IMapper mapper, ILogger<GetMovieCastByIdQueryHandler> logger)
        {
            _movieCastRepository = movieCastRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetMovieCastByIdQueryResult> Handle(GetMovieCastByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieCastRepository.GetByIdAsync(request.CastID, cancellationToken);
                return _mapper.Map<GetMovieCastByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the movie cast by ID");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
