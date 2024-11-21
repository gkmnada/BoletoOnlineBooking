using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Cinema.Commands;
using Ticket.Application.Features.Cinema.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Cinema.Handlers.CommandHandlers
{
    internal class UpdateCinemaCommandHandler : IRequestHandler<UpdateCinemaCommand, BaseResponse>
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCinemaCommandHandler> _logger;
        private readonly UpdateCinemaValidator _validator;

        public UpdateCinemaCommandHandler(ICinemaRepository cinemaRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCinemaCommandHandler> logger, UpdateCinemaValidator validator)
        {
            _cinemaRepository = cinemaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateCinemaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cinemaRepository.GetByIdAsync(request.id, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Cinema not found"
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

                await _cinemaRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Cinema updated successfully",
                    Data = new
                    {
                        id = values.id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating cinema");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
