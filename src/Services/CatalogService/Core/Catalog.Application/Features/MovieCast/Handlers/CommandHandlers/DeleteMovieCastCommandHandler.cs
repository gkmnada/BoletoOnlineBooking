using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieCast.Commands;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCast.Handlers.CommandHandlers
{
    public class DeleteMovieCastCommandHandler : IRequestHandler<DeleteMovieCastCommand, BaseResponse>
    {
        private readonly IMovieCastRepository _movieCastRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteMovieCastCommandHandler> _logger;

        public DeleteMovieCastCommandHandler(IMovieCastRepository movieCastRepository, IUnitOfWork unitOfWork, ILogger<DeleteMovieCastCommandHandler> logger)
        {
            _movieCastRepository = movieCastRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteMovieCastCommand request, CancellationToken cancellationToken)
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

                await _movieCastRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie cast deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting movie cast");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
