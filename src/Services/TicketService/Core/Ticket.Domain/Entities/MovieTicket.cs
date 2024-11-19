using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class MovieTicket : BaseEntity
    {
        public string session_id { get; set; }
        public Session session { get; set; }
        public string seat_id { get; set; }
        public Seat seat { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
    }
}
