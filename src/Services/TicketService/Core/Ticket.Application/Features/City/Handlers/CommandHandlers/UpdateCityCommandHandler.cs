using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using Ticket.Application.Common.Base;
using Ticket.Application.Features.City.Commands;
using Ticket.Application.Features.City.Validators;
using Ticket.Application.Interfaces.Repositories;

namespace Ticket.Application.Features.City.Handlers.CommandHandlers
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, BaseResponse>
    {
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCityCommandHandler> _logger;
        private readonly UpdateCityValidator _validator;

        public UpdateCityCommandHandler(ICityRepository cityRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCityCommandHandler> logger, UpdateCityValidator validator)
        {
            _cityRepository = cityRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _cityRepository.GetByIdAsync(request.CityID, cancellationToken);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "City not found"
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

                await _cityRepository.UpdateAsync(values);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "City updated successfully",
                    Data = new
                    {
                        CityID = values.CityID
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the city");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
