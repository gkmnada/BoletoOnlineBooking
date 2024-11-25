using Discount.API.Common.Base;
using Discount.API.Features.Coupon.Commands;
using Discount.API.Repositories;
using MediatR;

namespace Discount.API.Features.Coupon.Handlers.CommandHandlers
{
    public class DeleteCouponCommandHandler : IRequestHandler<DeleteCouponCommand, BaseResponse>
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<DeleteCouponCommandHandler> _logger;

        public DeleteCouponCommandHandler(ICouponRepository couponRepository, ILogger<DeleteCouponCommandHandler> logger)
        {
            _couponRepository = couponRepository;
            _logger = logger;
        }

        public async Task<BaseResponse> Handle(DeleteCouponCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var values = await _couponRepository.GetCouponByIdAsync(request.CouponID);

                if (values == null)
                {
                    return new BaseResponse
                    {
                        IsSuccess = false,
                        Message = "Coupon not found",
                    };
                }

                await _couponRepository.DeleteCouponAsync(values);

                return new BaseResponse
                {
                    IsSuccess = true,
                    Message = "Coupon deleted successfully",
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the coupon");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
