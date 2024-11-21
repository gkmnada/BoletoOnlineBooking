using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieCrew.Commands;
using Catalog.Application.Features.MovieCrew.Validators;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCrew.Handlers.CommandHandlers
{
    public class UpdateMovieCrewCommandHandler : IRequestHandler<UpdateMovieCrewCommand, BaseResponse>
    {
        private readonly IMovieCrewRepository _movieCrewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMovieCrewCommandHandler> _logger;
        private readonly UpdateMovieCrewValidator _validator;

        public UpdateMovieCrewCommandHandler(IMovieCrewRepository movieCrewRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateMovieCrewCommandHandler> logger, UpdateMovieCrewValidator validator)
        {
            _movieCrewRepository = movieCrewRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateMovieCrewCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieCrewRepository.GetByIdAsync(request.CrewID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Movie crew not found"
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

                _mapper.Map(request, values);

                await _movieCrewRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie crew updated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating movie crew");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
