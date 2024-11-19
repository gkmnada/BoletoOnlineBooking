using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Pricing : BaseEntity
    {
        public string session_id { get; set; }
        public Session session { get; set; }
        public string category_id { get; set; }
        public Category category { get; set; }
        public decimal price { get; set; }
    }
}
