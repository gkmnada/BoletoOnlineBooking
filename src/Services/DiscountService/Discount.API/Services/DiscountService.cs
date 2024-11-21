using Discount.API.protos;
using Discount.API.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace Discount.API.Services
{
    public class DiscountService : protos.DiscountService.DiscountServiceBase
    {
        private readonly ICouponRepository _couponRepository;
        private readonly ILogger<DiscountService> _logger;

        public DiscountService(ICouponRepository couponRepository, ILogger<DiscountService> logger)
        {
            _couponRepository = couponRepository;
            _logger = logger;
        }

        public override async Task<GetDiscountResponse> GetDiscount(GetDiscountRequest request, ServerCallContext context)
        {
            try
            {
                var values = await _couponRepository.GetCouponByCodeAsync(request.CouponCode);

                if (values == null)
                {
                    throw new Exception("Coupon not found");
                }

                var response = new GetDiscountResponse
                {
                    Discount = new GetDiscountModel
                    {
                        CouponCode = values.CouponCode,
                        Amount = values.Amount,
                        ExpirationDate = values.ExpirationDate.ToUniversalTime().ToTimestamp(),
                    }
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while getting the discount");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
