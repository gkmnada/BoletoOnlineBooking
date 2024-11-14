using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.Movie.Commands;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Movie.Handlers.CommandHandlers
{
    public class DeleteMovieCommandHandler : IRequestHandler<DeleteMovieCommand, BaseResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<DeleteMovieCommandHandler> _logger;
        private readonly IPublishEndpoint _publishEndpoint;

        public DeleteMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<DeleteMovieCommandHandler> logger, IPublishEndpoint publishEndpoint)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<BaseResponse> Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieRepository.GetByIdAsync(request.MovieID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Movie not found"
                    };
                }

                await _movieRepository.DeleteAsync(values);
                await _publishEndpoint.Publish(_mapper.Map<MovieDeleted>(values));
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _fileService.DeleteFileAsync(values.ImageURL);
                await _fileService.DeleteFileAsync(values.VideoURL);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie deleted successfully",
                    Data = values.MovieID
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the movie");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
