using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Pricing : BaseEntity
    {
        [Key]
        public string PricingID { get; set; } = Guid.NewGuid().ToString("D");
        public string SessionID { get; set; }
        public Session Session { get; set; }
        public string CategoryID { get; set; }
        public Category Category { get; set; }
        public decimal Price { get; set; }
    }
}
