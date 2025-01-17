﻿using AutoMapper;
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
    public class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, BaseResponse>
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IFileService _fileService;
        private readonly ILogger<CreateMovieCommandHandler> _logger;
        private readonly CreateMovieValidator _validator;
        private readonly IPublishEndpoint _publishEndpoint;

        public CreateMovieCommandHandler(IMovieRepository movieRepository, IUnitOfWork unitOfWork, IMapper mapper, IFileService fileService, ILogger<CreateMovieCommandHandler> logger, CreateMovieValidator validator, IPublishEndpoint publishEndpoint)
        {
            _movieRepository = movieRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileService = fileService;
            _logger = logger;
            _validator = validator;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<BaseResponse> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
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

                    var entity = _mapper.Map<Domain.Entities.Movie>(request);
                    entity.ImageURL = imageURL;

                    await _movieRepository.CreateAsync(entity);
                    await _publishEndpoint.Publish(_mapper.Map<MovieCreated>(entity));
                    await _unitOfWork.SaveChangesAsync(cancellationToken);

                    return new BaseResponse
                    {
                        IsSuccess = true,
                        Message = "Movie created successfully",
                        Data = entity.MovieID
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
                _logger.LogError(ex, "An error occurred while creating the movie");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
