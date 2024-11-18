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
    public class CreateMovieDetailCommandHandler : IRequestHandler<CreateMovieDetailCommand, BaseResponse>
    {
        private readonly IMovieDetailRepository _movieDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<CreateMovieDetailCommandHandler> _logger;
        private readonly CreateMovieDetailValidator _validator;

        public CreateMovieDetailCommandHandler(IMovieDetailRepository movieDetailRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<CreateMovieDetailCommandHandler> logger, CreateMovieDetailValidator validator)
        {
            _movieDetailRepository = movieDetailRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateMovieDetailCommand request, CancellationToken cancellationToken)
        {
            try
            {
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

                if (request.ImageURL.Length > 0 && request.VideoURL.Length > 0)
                {
                    var imageURL = await _fileService.UploadImageAsync(request.ImageURL);
                    var videoURL = await _fileService.UploadVideoAsync(request.VideoURL);

                    var entity = _mapper.Map<Domain.Entities.MovieDetail>(request);
                    entity.ImageURL = imageURL;
                    entity.VideoURL = videoURL;

                    await _movieDetailRepository.CreateAsync(entity);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Movie detail created successfully",
                    };
                }

                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Image URL and Video URL are required",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the movie detail");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
