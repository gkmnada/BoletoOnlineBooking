namespace Payment.API.Models
{
    public class CreateOrderRequest
    {
        public string MovieID { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string CouponCode { get; set; }
        public int DiscountAmount { get; set; }
        public string UserID { get; set; }
        public List<CreateOrderDetailRequest> OrderDetails { get; set; }
    }

    public class CreateOrderDetailRequest
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
