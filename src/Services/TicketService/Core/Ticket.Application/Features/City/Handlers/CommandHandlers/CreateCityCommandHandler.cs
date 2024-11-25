using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.City.Commands;
using Ticket.Application.Features.City.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.City.Handlers.CommandHandlers
{
    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, BaseResponse>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCityCommandHandler> _logger;
        private readonly CreateCityValidator _validator;

        public CreateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateCityCommandHandler> logger, CreateCityValidator validator)
        {
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateCityCommand request, CancellationToken cancellationToken)
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

                var entity = _mapper.Map<Domain.Entities.City>(request);

                await _cityRepository.CreateAsync(entity);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "City created successfully",
                    Data = new
                    {
                        CityID = entity.CityID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the city");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
