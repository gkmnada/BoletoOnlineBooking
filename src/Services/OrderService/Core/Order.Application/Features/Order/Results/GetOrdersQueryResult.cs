namespace Order.Application.Features.Order.Results
{
    public class GetOrdersQueryResult
    {
        public string OrderID { get; set; }
        public string MovieName { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string CouponCode { get; set; }
        public int DiscountAmount { get; set; }
        public string UserID { get; set; }
    }
}
