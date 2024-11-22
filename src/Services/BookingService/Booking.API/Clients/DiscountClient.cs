using Booking.API.Models;
using Grpc.Core;
using Grpc.Net.Client;

namespace Booking.API.Clients
{
    public class DiscountClient
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<DiscountClient> _logger;

        public DiscountClient(IConfiguration configuration, ILogger<DiscountClient> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public DiscountModel GetDiscount(string couponCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(couponCode))
                {
                    throw new ArgumentException("Coupon code is required");
                }

                var channel = GrpcChannel.ForAddress(_configuration["DiscountGRPC"] ?? "DiscountGRPC");
                var client = new protos.DiscountService.DiscountServiceClient(channel);

                var request = new protos.GetDiscountRequest { CouponCode = couponCode };
                var response = client.GetDiscount(request);

                return new DiscountModel
                {
                    CouponCode = response.Discount.CouponCode,
                    Amount = response.Discount.Amount,
                    ExpirationDate = response.Discount.ExpirationDate.ToDateTime()
                };
            }
            catch (RpcException rpcEx)
            {
                _logger.LogError(rpcEx, "Coupon code is invalid");
                throw new Exception("Coupon code is invalid", rpcEx);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching the discount");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
