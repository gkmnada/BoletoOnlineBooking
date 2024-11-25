using AutoMapper;
using Discount.API.Common.Base;
using Discount.API.Features.Coupon.Commands;
using Discount.API.Features.Coupon.Validators;
using Discount.API.Repositories;
using MediatR;

namespace Discount.API.Features.Coupon.Handlers.CommandHandlers
{
    public class CreateCouponCommandHandler : IRequestHandler<CreateCouponCommand, BaseResponse>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCouponCommandHandler> _logger;
        private readonly CreateCouponValidator _validator;

        public CreateCouponCommandHandler(ICouponRepository couponRepository, IMapper mapper, ILogger<CreateCouponCommandHandler> logger, CreateCouponValidator validator)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request);

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

                var entity = _mapper.Map<Entities.Coupon>(request);

                await _couponRepository.CreateCouponAsync(entity);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Coupon created successfully",
                    Data = new
                    {
                        code = entity.CouponCode
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating the coupon");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
