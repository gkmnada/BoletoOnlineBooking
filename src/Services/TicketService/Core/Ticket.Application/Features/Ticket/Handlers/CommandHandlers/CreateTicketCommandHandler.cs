using AutoMapper;
using Boleto.Contracts.Enums.Tickets;
using Boleto.Contracts.Events.TicketEvents;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Ticket.Commands;
using Ticket.Application.Features.Ticket.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Ticket.Handlers.CommandHandlers
{
    public class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, BaseResponse>
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly ISessionRepository _sessionRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateTicketCommandHandler> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CreateTicketValidator _validator;
        private string _userID;

        public CreateTicketCommandHandler(ITicketRepository ticketRepository, ISessionRepository sessionRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateTicketCommandHandler> logger, IPublishEndpoint publishEndpoint, IHttpContextAccessor httpContextAccessor, CreateTicketValidator validator)
        {
            _ticketRepository = ticketRepository;
            _sessionRepository = sessionRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
            _userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public async Task<BaseResponse> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
        {
            try
            {
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

                var entity = _mapper.Map<Domain.Entities.Ticket>(request);
                entity.UserID = _userID;
                entity.Status = TicketStatus.Pending.ToString();

                var session = await _sessionRepository.GetSessionAsync(entity.SessionID, cancellationToken);
                entity.Session = session;

                await _ticketRepository.CreateAsync(entity);
                await _publishEndpoint.Publish(_mapper.Map<TicketCreated>(entity));
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Ticket created successfully",
                    Data = new
                    {
                        TicketID = entity.TicketID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the ticket");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
