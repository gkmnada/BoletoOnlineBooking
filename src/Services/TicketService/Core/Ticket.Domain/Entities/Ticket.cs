using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        [Key]
        public string TicketID { get; set; } = Guid.NewGuid().ToString("D");
        public string SessionID { get; set; }
        public Session Session { get; set; }
        public string SeatID { get; set; }
        public Seat Seat { get; set; }
        public string Status { get; set; }
        public decimal Price { get; set; }
        public string UserID { get; set; }
    }
}
