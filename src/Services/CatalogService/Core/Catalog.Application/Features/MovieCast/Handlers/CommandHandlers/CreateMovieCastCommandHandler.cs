using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieCast.Commands;
using Catalog.Application.Features.MovieCast.Validators;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCast.Handlers.CommandHandlers
{
    public class CreateMovieCastCommandHandler : IRequestHandler<CreateMovieCastCommand, BaseResponse>
    {
        private readonly IMovieCastRepository _movieCastRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMovieCastCommandHandler> _logger;
        private readonly CreateMovieCastValidator _validator;

        public CreateMovieCastCommandHandler(IMovieCastRepository movieCastRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateMovieCastCommandHandler> logger, CreateMovieCastValidator validator)
        {
            _movieCastRepository = movieCastRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateMovieCastCommand request, CancellationToken cancellationToken)
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

                foreach (var item in request.MovieCasts)
                {
                    var entity = new Domain.Entities.MovieCast
                    {
                        CastName = item.CastName,
                        Character = item.Character,
                        ImageURL = item.ImageURL,
                        MovieID = item.MovieID
                    };

                    await _movieCastRepository.CreateAsync(entity);
                }

                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie casts created successfully",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the movie cast");
                throw new Exception("An error occurred while processing your request", ex);
            }
        }
    }
}
