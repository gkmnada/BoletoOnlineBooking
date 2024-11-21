using Catalog.Application.Common.Base;
using Catalog.Application.Features.MovieCrew.Commands;
using Catalog.Application.Interfaces.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Catalog.Application.Features.MovieCrew.Handlers.CommandHandlers
{
    public class DeleteMovieCrewCommandHandler : IRequestHandler<DeleteMovieCrewCommand, BaseResponse>
    {
        private readonly IMovieCrewRepository _movieCrewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteMovieCrewCommandHandler> _logger;

        public DeleteMovieCrewCommandHandler(IMovieCrewRepository movieCrewRepository, IUnitOfWork unitOfWork, ILogger<DeleteMovieCrewCommandHandler> logger)
        {
            _movieCrewRepository = movieCrewRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteMovieCrewCommand request, CancellationToken cancellationToken)
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

                await _movieCrewRepository.DeleteAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie crew deleted successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the movie crew");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
