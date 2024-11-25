using AutoMapper;
using Discount.API.Common.Base;
using Discount.API.Features.Coupon.Commands;
using Discount.API.Features.Coupon.Validators;
using Discount.API.Repositories;
using MediatR;

namespace Discount.API.Features.Coupon.Handlers.CommandHandlers
{
    public class UpdateCouponCommandHandler : IRequestHandler<UpdateCouponCommand, BaseResponse>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCouponCommandHandler> _logger;
        private readonly UpdateCouponValidator _validator;

        public UpdateCouponCommandHandler(ICouponRepository couponRepository, IMapper mapper, ILogger<UpdateCouponCommandHandler> logger, UpdateCouponValidator validator)
        {
            _couponRepository = couponRepository;
            _mapper = mapper;
            _logger = logger;
            _validator = validator;
        }

        public async Task<BaseResponse> Handle(UpdateCouponCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _couponRepository.GetCouponByIdAsync(request.CouponID);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon not found"
                    };
                }

                var validationResult = await _validator.ValidateAsync(request);

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

                await _couponRepository.UpdateCouponAsync(values);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Coupon updated successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating the coupon");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
