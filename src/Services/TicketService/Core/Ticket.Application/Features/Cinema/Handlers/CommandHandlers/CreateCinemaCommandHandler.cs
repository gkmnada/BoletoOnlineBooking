using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.Cinema.Commands;
using Ticket.Application.Features.Cinema.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.Cinema.Handlers.CommandHandlers
{
    public class CreateCinemaCommandHandler : IRequestHandler<CreateCinemaCommand, BaseResponse>
    {
        private readonly ICinemaRepository _cinemaRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCinemaCommandHandler> _logger;
        private readonly CreateCinemaValidator _validator;

        public CreateCinemaCommandHandler(ICinemaRepository cinemaRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateCinemaCommandHandler> logger, CreateCinemaValidator validator)
        {
            _cinemaRepository = cinemaRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateCinemaCommand request, CancellationToken cancellationToken)
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

                var entity = _mapper.Map<Domain.Entities.Cinema>(request);

                await _cinemaRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Cinema created successfully",
                    Data = new
                    {
                        CinemaID = entity.CinemaID,
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the cinema");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
