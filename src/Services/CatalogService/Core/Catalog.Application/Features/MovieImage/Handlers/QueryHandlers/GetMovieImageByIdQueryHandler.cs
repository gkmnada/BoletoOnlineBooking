using AutoMapper;
using Catalog.Application.Features.MovieImage.Queries;
using Catalog.Application.Features.MovieImage.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieImage.Handlers.QueryHandlers
{
    public class GetMovieImageByIdQueryHandler : IRequestHandler<GetMovieImageByIdQuery, GetMovieImageByIdQueryResult>
    {
        private readonly IMovieImageRepository _movieImageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieImageByIdQueryHandler> _logger;

        public GetMovieImageByIdQueryHandler(IMovieImageRepository movieImageRepository, IMapper mapper, ILogger<GetMovieImageByIdQueryHandler> logger)
        {
            _movieImageRepository = movieImageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<GetMovieImageByIdQueryResult> Handle(GetMovieImageByIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieImageRepository.GetByIdAsync(request.ImageID, cancellationToken);
                return _mapper.Map<GetMovieImageByIdQueryResult>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching movie image by ID");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
