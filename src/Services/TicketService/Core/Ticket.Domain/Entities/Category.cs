using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Key]
        public string CategoryID { get; set; } = Guid.NewGuid().ToString("D");
        public string Name { get; set; }
        public List<Pricing> Pricings { get; set; }
    }
}
