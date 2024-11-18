using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieDetail.Commands;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieDetail.Handlers.CommandHandlers
{
    public class DeleteMovieDetailCommandHandler : IRequestHandler<DeleteMovieDetailCommand, BaseResponse>
    {
        private readonly IMovieDetailRepository _movieDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly ILogger<DeleteMovieDetailCommandHandler> _logger;

        public DeleteMovieDetailCommandHandler(IMovieDetailRepository movieDetailRepository, IUnitOfWork unitOfWork, IFileService fileService, ILogger<DeleteMovieDetailCommandHandler> logger)
        {
            _movieDetailRepository = movieDetailRepository;
            _unitOfWork = unitOfWork;
            _fileService = fileService;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteMovieDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieDetailRepository.GetByIdAsync(request.DetailID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Movie detail not found"
                    };
                }

                await _movieDetailRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await _fileService.DeleteFileAsync(values.ImageURL);
                await _fileService.DeleteFileAsync(values.VideoURL);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie detail deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the movie detail");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
