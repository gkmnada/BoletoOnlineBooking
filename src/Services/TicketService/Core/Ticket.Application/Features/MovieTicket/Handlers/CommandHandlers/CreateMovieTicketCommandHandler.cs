using AutoMapper;
using Boleto.Contracts.Events.TicketEvents;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.MovieTicket.Commands;
using Ticket.Application.Features.MovieTicket.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.MovieTicket.Handlers.CommandHandlers
{
    public class CreateMovieTicketCommandHandler : IRequestHandler<CreateMovieTicketCommand, BaseResponse>
    {
        private readonly IMovieTicketRepository _movieTicketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateMovieTicketCommandHandler> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly CreateMovieTicketValidator _validator;
        private string _userID;

        public CreateMovieTicketCommandHandler(IMovieTicketRepository movieTicketRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateMovieTicketCommandHandler> logger, IPublishEndpoint publishEndpoint, IHttpContextAccessor httpContextAccessor, CreateMovieTicketValidator validator)
        {
            _movieTicketRepository = movieTicketRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _publishEndpoint = publishEndpoint;
            _httpContextAccessor = httpContextAccessor;
            _validator = validator;
            _userID = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier).Value;
        }

        public async Task<BaseResponse> Handle(CreateMovieTicketCommand request, CancellationToken cancellationToken)
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

                var entity = _mapper.Map<Domain.Entities.MovieTicket>(request);
                entity.user_id = _userID;

                await _movieTicketRepository.CreateAsync(entity);
                await _publishEndpoint.Publish(_mapper.Map<MovieTicketCreated>(entity));
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Movie ticket created successfully",
                    Data = new
                    {
                        id = entity.ticket_id
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the movie ticket");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
