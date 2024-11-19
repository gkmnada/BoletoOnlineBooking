using System.ComponentModel.DataAnnotations;

namespace Ticket.Domain.Common.Base
{
    public class BaseEntity
    {
        [Key]
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public bool is_active { get; set; }

        public BaseEntity()
        {
            created_at = DateTime.UtcNow;
            is_active = true;
            id = Guid.NewGuid().ToString("D");
        }
    }
}
