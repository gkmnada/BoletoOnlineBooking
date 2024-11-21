using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Hall.Commands;
using Ticket.Application.Features.Hall.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Hall.Handlers.CommandHandlers
{
    public class CreateHallCommandHandler : IRequestHandler<CreateHallCommand, BaseResponse>
    {
        private readonly IHallRepository _hallRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateHallCommandHandler> _logger;
        private readonly CreateHallValidator _validator;

        public CreateHallCommandHandler(IHallRepository hallRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateHallCommandHandler> logger, CreateHallValidator validator)
        {
            _hallRepository = hallRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateHallCommand request, CancellationToken cancellationToken)
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

                var entity = _mapper.Map<Domain.Entities.Hall>(request);

                await _hallRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Hall created successfully",
                    Data = new
                    {
                        id = entity.id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the hall");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
