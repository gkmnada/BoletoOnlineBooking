using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Seat.Commands;
using Ticket.Application.Features.Seat.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Seat.Handlers.CommandHandlers
{
    public class UpdateSeatCommandHandler : IRequestHandler<UpdateSeatCommand, BaseResponse>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateSeatCommandHandler> _logger;
        private readonly UpdateSeatValidator _validator;

        public UpdateSeatCommandHandler(ISeatRepository seatRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateSeatCommandHandler> logger, UpdateSeatValidator validator)
        {
            _seatRepository = seatRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateSeatCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _seatRepository.GetByIdAsync(request.SeatID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Seat not found",
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

                _mapper.Map(request, values);

                await _seatRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Seat updated successfully",
                    Data = new
                    {
                        SeatID = values.SeatID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the seat");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
