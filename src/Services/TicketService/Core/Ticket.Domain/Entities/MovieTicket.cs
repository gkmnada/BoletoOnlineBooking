using System.ComponentModel.DataAnnotations;

namespace Ticket.Domain.Entities
{
    public class MovieTicket
    {
        [Key]
        public string ticket_id { get; set; } = Guid.NewGuid().ToString("D");
        public string session_id { get; set; }
        public Session session { get; set; }
        public string seat_id { get; set; }
        public Seat seat { get; set; }
        public string status { get; set; }
        public decimal price { get; set; }
        public DateTime created_at { get; set; } = DateTime.Now;
    }
}
