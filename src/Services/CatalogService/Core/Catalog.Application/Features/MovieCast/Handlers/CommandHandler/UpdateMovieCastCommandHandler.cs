using AutoMapper;
using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieCast.Commands;
using Catalog.Application.Features.MovieCast.Validators;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCast.Handlers.CommandHandler
{
    public class UpdateMovieCastCommandHandler : IRequestHandler<UpdateMovieCastCommand, BaseResponse>
    {
        private readonly IMovieCastRepository _movieCastRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateMovieCastCommandHandler> _logger;
        private readonly UpdateMovieCastValidator _validator;

        public UpdateMovieCastCommandHandler(IMovieCastRepository movieCastRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateMovieCastCommandHandler> logger, UpdateMovieCastValidator validator)
        {
            _movieCastRepository = movieCastRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateMovieCastCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _movieCastRepository.GetByIdAsync(request.CastID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Movie cast not found"
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

                await _movieCastRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie cast updated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating movie cast");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
