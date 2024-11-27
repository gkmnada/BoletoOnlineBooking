using Order.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Entities
{
    public class Order : BaseEntity
    {
        [Key]
        public string OrderID { get; set; } = Guid.NewGuid().ToString("D");
        public string MovieID { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string CouponCode { get; set; }
        public int DiscountAmount { get; set; }
        public string UserID { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
