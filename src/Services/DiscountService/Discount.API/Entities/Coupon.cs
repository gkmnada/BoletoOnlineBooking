using System.ComponentModel.DataAnnotations;

namespace Discount.API.Entities
{
    public class Coupon
    {
        [Key]
        public string CouponID { get; set; } = Guid.NewGuid().ToString("D");
        public string CouponCode { get; set; }
        public int Amount { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddDays(7);
        public bool IsActive { get; set; } = true;
    }
}
