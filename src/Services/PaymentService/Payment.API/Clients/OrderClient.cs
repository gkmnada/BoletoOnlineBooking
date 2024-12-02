using Grpc.Net.Client;
using Payment.API.Models;
using Payment.API.protos;

namespace Payment.API.Clients
{
    public class OrderClient
    {
        private readonly protos.OrderService.OrderServiceClient _client;
        private readonly ILogger<OrderClient> _logger;

        public OrderClient(IConfiguration configuration, ILogger<OrderClient> logger)
        {
            _logger = logger;
            var channel = GrpcChannel.ForAddress(configuration["OrderGRPC"] ?? "");
            _client = new protos.OrderService.OrderServiceClient(channel);
        }

        public async Task<protos.GetOrderResponse> CreateOrder(CreateOrderRequest createOrderRequest)
        {
            try
            {
                var request = new GetOrderRequest
                {
                    MovieID = createOrderRequest.MovieID,
                    Status = createOrderRequest.Status,
                    TotalAmount = (double)createOrderRequest.TotalAmount,
                    CouponCode = createOrderRequest.CouponCode,
                    DiscountAmount = createOrderRequest.DiscountAmount,
                    UserID = createOrderRequest.UserID,
                };

                if (createOrderRequest.OrderDetails != null)
                {
                    foreach (var detail in createOrderRequest.OrderDetails)
                    {
                        request.OrderDetails.Add(new GetOrderDetailRequest
                        {
                            MovieID = detail.MovieID,
                            CinemaID = detail.CinemaID,
                            HallID = detail.HallID,
                            SessionID = detail.SessionID,
                            SeatID = detail.SeatID,
                            Status = detail.Status,
                            Price = (double)detail.Price
                        });
                    }
                }

                var response = await _client.GetOrderAsync(request);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while creating order");
                throw new Exception("An error occurred while processing the request", ex);
            }
        }
    }
}
