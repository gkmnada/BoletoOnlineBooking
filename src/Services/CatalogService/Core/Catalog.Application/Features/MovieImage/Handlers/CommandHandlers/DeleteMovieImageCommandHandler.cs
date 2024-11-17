using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieImage.Commands;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieImage.Handlers.CommandHandlers
{
    public class DeleteMovieImageCommandHandler : IRequestHandler<DeleteMovieImageCommand, BaseResponse>
    {
        private readonly IMovieImageRepository _movieImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly ILogger<DeleteMovieImageCommandHandler> _logger;

        public DeleteMovieImageCommandHandler(IMovieImageRepository movieImageRepository, IUnitOfWork unitOfWork, IFileService fileService, ILogger<DeleteMovieImageCommandHandler> logger)
        {
            _movieImageRepository = movieImageRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteMovieImageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieImageRepository.GetByIdAsync(request.ImageID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Movie image not found"
                    };
                }

                await _fileService.DeleteFileAsync(values.ImageURL);

                await _movieImageRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie image deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the movie image");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
