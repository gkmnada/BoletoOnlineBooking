using MediatR;
using Order.Application.Common.Base;

namespace Order.Application.Features.Order.Commands
{
    public class CreateOrderWithDetailCommand : IRequest<BaseResponse>
    {
        public string MovieID { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string CouponCode { get; set; }
        public int DiscountAmount { get; set; }
        public string UserID { get; set; }
        public List<CreateOrderDetailCommand> OrderDetails { get; set; }
    }

    public class CreateOrderDetailCommand
    {
        public string MovieID { get; set; }
        public string CinemaID { get; set; }
        public string HallID { get; set; }
        public string SessionID { get; set; }
        public string SeatID { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
    }
}
