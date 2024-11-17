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
    public class CreateMovieImageCommandHandler : IRequestHandler<CreateMovieImageCommand, BaseResponse>
    {
        private readonly IMovieImageRepository _movieImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<CreateMovieImageCommandHandler> _logger;
        private readonly CreateMovieImageValidator _validator;

        public CreateMovieImageCommandHandler(IMovieImageRepository movieImageRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<CreateMovieImageCommandHandler> logger, CreateMovieImageValidator validator)
        {
            _movieImageRepository = movieImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateMovieImageCommand request, CancellationToken cancellationToken)
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

                if (request.ImageURL.Length > 0)
                {
                    var imageURL = await _fileService.UploadImageAsync(request.ImageURL);

                    var entity = _mapper.Map<Domain.Entities.MovieImage>(request);
                    entity.ImageURL = imageURL;

                    await _movieImageRepository.CreateAsync(entity);
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Movie image created successfully"
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
                _logger.LogError(ex, "An error occurred while creating the movie image");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
