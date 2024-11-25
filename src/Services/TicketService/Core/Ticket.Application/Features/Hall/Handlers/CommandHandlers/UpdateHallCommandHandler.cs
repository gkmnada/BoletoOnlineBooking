using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Hall.Commands;
using Ticket.Application.Features.Hall.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Hall.Handlers.CommandHandlers
{
    public class UpdateHallCommandHandler : IRequestHandler<UpdateHallCommand, BaseResponse>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateHallCommandHandler> _logger;
        private readonly UpdateHallValidator _validator;

        public UpdateHallCommandHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateHallCommandHandler> logger, UpdateHallValidator validator)
        {
            _hallRepository = hallRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateHallCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _hallRepository.GetByIdAsync(request.HallID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Hall not found"
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

                await _hallRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Hall updated successfully",
                    Data = new
                    {
                        HallID = values.HallID,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating hall");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
