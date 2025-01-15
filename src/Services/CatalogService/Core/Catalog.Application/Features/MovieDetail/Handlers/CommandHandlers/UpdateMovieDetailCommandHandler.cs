using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieDetail.Commands;
using Catalog.Application.Features.MovieDetail.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieDetail.Handlers.CommandHandlers
{
    public class UpdateMovieDetailCommandHandler : IRequestHandler<UpdateMovieDetailCommand, BaseResponse>
    {
        private readonly IMovieDetailRepository _movieDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<UpdateMovieDetailCommandHandler> _logger;
        private readonly UpdateMovieDetailValidator _validator;

        public UpdateMovieDetailCommandHandler(IMovieDetailRepository movieDetailRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<UpdateMovieDetailCommandHandler> logger, UpdateMovieDetailValidator validator)
        {
            _movieDetailRepository = movieDetailRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateMovieDetailCommand request, CancellationToken cancellationToken)
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

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(x => x.ErrorMessage).ToList();

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Validation failed",
                        Errors = errors
                    };
                }

                if (request.ImageURL != null && request.ImageURL.Length > 0 && request.VideoURL != null && request.VideoURL.Length > 0)
                {
                    await _fileService.DeleteFileAsync(values.ImageURL);
                    await _fileService.DeleteFileAsync(values.VideoURL);

                    var imageURL = await _fileService.UploadImageAsync(request.ImageURL);
                    var videoURL = await _fileService.UploadVideoAsync(request.VideoURL);

                    var entity = _mapper.Map(request, values);
                    entity.ImageURL = imageURL;
                    entity.VideoURL = videoURL;

                    await _movieDetailRepository.UpdateAsync(values);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Movie detail updated successfully"
                    };
                }
                else if (!string.IsNullOrEmpty(request.ExistingImageURL) && !string.IsNullOrEmpty(request.ExistingVideoURL))
                {
                    var entity = _mapper.Map(request, values);
                    entity.ImageURL = request.ExistingImageURL;
                    entity.VideoURL = request.ExistingVideoURL;

                    await _movieDetailRepository.UpdateAsync(values);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Movie detail updated successfully"
                    };
                }

                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Image URL and Video URL are required"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating movie detail");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
