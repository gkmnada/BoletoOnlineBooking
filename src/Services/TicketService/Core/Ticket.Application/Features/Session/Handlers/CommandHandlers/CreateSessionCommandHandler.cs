using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Session.Commands;
using Ticket.Application.Features.Session.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Session.Handlers.CommandHandlers
{
    public class CreateSessionCommandHandler : IRequestHandler<CreateSessionCommand, BaseResponse>
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateSessionCommandHandler> _logger;
        private readonly CreateSessionValidator _validator;

        public CreateSessionCommandHandler(ISessionRepository sessionRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateSessionCommandHandler> logger, CreateSessionValidator validator)
        {
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateSessionCommand request, CancellationToken cancellationToken)
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

                var entity = _mapper.Map<Domain.Entities.Session>(request);

                await _sessionRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Session created successfully",
                    Data = new
                    {
                        id = entity.id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the session");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
