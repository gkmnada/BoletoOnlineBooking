using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieImage.Commands;
using Catalog.Application.Features.MovieImage.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieImage.Handlers.CommandHandlers
{
    public class UpdateMovieImageCommandHandler : IRequestHandler<UpdateMovieImageCommand, BaseResponse>
    {
        private readonly IMovieImageRepository _movieImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<UpdateMovieImageCommandHandler> _logger;
        private readonly UpdateMovieImageValidator _validator;

        public UpdateMovieImageCommandHandler(IMovieImageRepository movieImageRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<UpdateMovieImageCommandHandler> logger, UpdateMovieImageValidator validator)
        {
            _movieImageRepository = movieImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateMovieImageCommand request, CancellationToken cancellationToken)
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

                var validationResult = await _validator.ValidateAsync(request, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();

                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Validation failed",
                        Errors = errors
                    };
                }

                if (request.ImageURL.Length > 0)
                {
                    await _fileService.DeleteFileAsync(values.ImageURL);

                    var imageURL = await _fileService.UploadImageAsync(request.ImageURL);

                    var entity = _mapper.Map(request, values);
                    entity.ImageURL = imageURL;

                    await _movieImageRepository.UpdateAsync(entity);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Movie image updated successfully"
                    };
                }

                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Image URL is required"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the movie image");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
