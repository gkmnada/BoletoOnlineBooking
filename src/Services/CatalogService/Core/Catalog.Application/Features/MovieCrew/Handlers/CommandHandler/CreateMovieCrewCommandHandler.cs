using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieCrew.Commands;
using Catalog.Application.Features.MovieCrew.Validators;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCrew.Handlers.CommandHandler
{
    public class CreateMovieCrewCommandHandler : IRequestHandler<CreateMovieCrewCommand, BaseResponse>
    {
        private readonly IMovieCrewRepository _movieCrewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMovieCrewCommandHandler> _logger;
        private readonly CreateMovieCrewValidator _validator;

        public CreateMovieCrewCommandHandler(IMovieCrewRepository movieCrewRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateMovieCrewCommandHandler> logger, CreateMovieCrewValidator validator)
        {
            _movieCrewRepository = movieCrewRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateMovieCrewCommand request, CancellationToken cancellationToken)
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

                var entity = _mapper.Map<Domain.Entities.MovieCrew>(request);

                await _movieCrewRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie crew created successfully",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the movie crew");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
