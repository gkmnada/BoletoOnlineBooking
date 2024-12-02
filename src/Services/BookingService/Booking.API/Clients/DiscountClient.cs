using Booking.API.Models;
using Grpc.Core;
using Grpc.Net.Client;

namespace Booking.API.Clients
{
    public class DiscountClient
    {
        private readonly protos.DiscountService.DiscountServiceClient _client;
        private readonly ILogger<DiscountClient> _logger;

        public DiscountClient(IConfiguration configuration, ILogger<DiscountClient> logger)
        {
            _logger = logger;
            var channel = GrpcChannel.ForAddress(configuration["DiscountGRPC"] ?? "");
            _client = new protos.DiscountService.DiscountServiceClient(channel);
        }

        public DiscountModel GetDiscount(string couponCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(couponCode))
                {
                    throw new ArgumentException("Coupon code is required");
                }

                var request = new protos.GetDiscountRequest { CouponCode = couponCode };
                var response = _client.GetDiscount(request);

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
