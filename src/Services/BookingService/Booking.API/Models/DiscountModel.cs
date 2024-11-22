namespace Booking.API.Models
{
    public class DiscountModel
    {
        public string CouponCode { get; set; }
        public int Amount { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
