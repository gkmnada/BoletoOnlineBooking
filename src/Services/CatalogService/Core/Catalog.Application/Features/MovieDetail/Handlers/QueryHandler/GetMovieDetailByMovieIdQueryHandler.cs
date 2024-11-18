using AutoMapper;
using Catalog.Application.Features.MovieDetail.Queries;
using Catalog.Application.Features.MovieDetail.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieDetail.Handlers.QueryHandler
{
    public class GetMovieDetailByMovieIdQueryHandler : IRequestHandler<GetMovieDetailByMovieIdQuery, GetMovieDetailByMovieIdQueryResult>
    {
        private readonly IMovieDetailRepository _movieDetailRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieDetailByMovieIdQueryHandler> _logger;

        public GetMovieDetailByMovieIdQueryHandler(IMovieDetailRepository movieDetailRepository, IMapper mapper, ILogger<GetMovieDetailByMovieIdQueryHandler> logger)
        {
            _movieDetailRepository = movieDetailRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetMovieDetailByMovieIdQueryResult> Handle(GetMovieDetailByMovieIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieDetailRepository.GetByFilterAsync(x => x.MovieID == request.MovieID, cancellationToken);
                return _mapper.Map<GetMovieDetailByMovieIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting movie detail by movie ID");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
