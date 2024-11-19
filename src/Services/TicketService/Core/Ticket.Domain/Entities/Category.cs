using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string name { get; set; }
        public List<Pricing> pricings { get; set; }
    }
}
