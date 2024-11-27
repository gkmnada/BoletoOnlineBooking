using Order.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Order.Domain.Entities
{
    public class OrderDetail : BaseEntity
    {
        [Key]
        public string DetailID { get; set; } = Guid.NewGuid().ToString("D");
        public string OrderID { get; set; }
        public string MovieID { get; set; }
        public string CinemaID { get; set; }
        public string HallID { get; set; }
        public string SessionID { get; set; }
        public string SeatID { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public Order Order { get; set; }
    }
}
