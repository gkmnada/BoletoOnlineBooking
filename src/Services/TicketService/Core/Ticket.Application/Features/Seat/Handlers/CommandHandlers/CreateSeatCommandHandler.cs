using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Seat.Commands;
using Ticket.Application.Features.Seat.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Seat.Handlers.CommandHandlers
{
    public class CreateSeatCommandHandler : IRequestHandler<CreateSeatCommand, BaseResponse>
    {
        private readonly ISeatRepository _seatRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSeatCommandHandler> _logger;
        private readonly CreateSeatValidator _validator;

        public CreateSeatCommandHandler(ISeatRepository seatRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateSeatCommandHandler> logger, CreateSeatValidator validator)
        {
            _seatRepository = seatRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateSeatCommand request, CancellationToken cancellationToken)
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

                var entity = _mapper.Map<Domain.Entities.Seat>(request);

                await _seatRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Seat created successfully",
                    Data = new
                    {
                        id = entity.id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the seat");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
