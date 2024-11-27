using Grpc.Core;
using MediatR;
using Order.API.protos;
using Order.Application.Features.Order.Commands;

namespace Order.API.Services
{
    public class OrderService : protos.OrderService.OrderServiceBase
    {
        private readonly IMediator _mediator;

        public OrderService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<GetOrderResponse> GetOrder(GetOrderRequest request, ServerCallContext context)
        {
            var command = new CreateOrderWithDetailCommand
            {
                MovieID = request.MovieID,
                Status = request.Status,
                TotalAmount = (decimal)request.TotalAmount,
                CouponCode = request.CouponCode,
                DiscountAmount = request.DiscountAmount,
                UserID = request.UserID,
                OrderDetails = request.OrderDetails.Select(detail => new CreateOrderDetailCommand
                {
                    MovieID = detail.MovieID,
                    CinemaID = detail.CinemaID,
                    HallID = detail.HallID,
                    SessionID = detail.SessionID,
                    SeatID = detail.SeatID,
                    Status = detail.Status,
                    Price = (decimal)detail.Price
                }).ToList()
            };

            var baseResponse = await _mediator.Send(command);

            var response = new GetOrderResponse
            {
                Success = baseResponse.IsSuccess,
                Message = baseResponse.Message,
                OrderID = baseResponse.Data.ToString()
            };

            return response;
        }
    }
}
