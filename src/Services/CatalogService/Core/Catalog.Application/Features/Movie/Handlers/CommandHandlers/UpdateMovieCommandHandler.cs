using AutoMapper;
using Boleto.Contracts.Events.MovieEvents;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.Movie.Commands;
using Catalog.Application.Features.Movie.Validators;
using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.Movie.Handlers.CommandHandlers
{
    public class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, BaseResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<UpdateMovieCommandHandler> _logger;
        private readonly UpdateMovieValidator _validator;
        private readonly IPublishEndpoint _publishEndpoint;

        public UpdateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<UpdateMovieCommandHandler> logger, UpdateMovieValidator validator, IPublishEndpoint publishEndpoint)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _validator = validator;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<BaseResponse> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieRepository.GetByIdAsync(request.MovieID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        Message = "Movie not found",
                        IsSuccess = false
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

                if (request.ImageURL.Length > 0 && request.VideoURL.Length > 0)
                {
                    await _fileService.DeleteFileAsync(values.ImageURL);
                    await _fileService.DeleteFileAsync(values.VideoURL);

                    var imageURL = await _fileService.UploadImageAsync(request.ImageURL);
                    var videoURL = await _fileService.UploadVideoAsync(request.VideoURL);

                    var entity = _mapper.Map(request, values);
                    entity.ImageURL = imageURL;
                    entity.VideoURL = videoURL;

                    await _movieRepository.UpdateAsync(values);
                    await _publishEndpoint.Publish(_mapper.Map<MovieUpdated>(values));
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Movie updated successfully",
                        Data = values.MovieID
                    };
                }

                return new BaseResponse
                {
                    IsSuccess = false,
                    Message = "Image and Video are required"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the movie");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
