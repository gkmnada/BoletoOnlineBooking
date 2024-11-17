using AutoMapper;
using Catalog.Application.Features.MovieImage.Queries;
using Catalog.Application.Features.MovieImage.Results;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieImage.Handlers.QueryHandlers
{
    public class GetMovieImagesQueryHandler : IRequestHandler<GetMovieImagesQuery, List<GetMovieImagesQueryResult>>
    {
        private readonly IMovieImageRepository _movieImageRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<GetMovieImagesQueryHandler> _logger;

        public GetMovieImagesQueryHandler(IMovieImageRepository movieImageRepository, IMapper mapper, ILogger<GetMovieImagesQueryHandler> logger)
        {
            _movieImageRepository = movieImageRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<List<GetMovieImagesQueryResult>> Handle(GetMovieImagesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieImageRepository.ListMovieImagesAsync(request.MovieID, cancellationToken);
                return _mapper.Map<List<GetMovieImagesQueryResult>>(values);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching movie images");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
