using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Session : BaseEntity
    {
        public DateOnly session_date { get; set; }
        public TimeOnly session_time { get; set; }
        public string hall_id { get; set; }
        public Hall hall { get; set; }
        public string cinema_id { get; set; }
        public Cinema cinema { get; set; }
        public string movie_id { get; set; }
        public List<MovieTicket> movie_tickets { get; set; }
        public List<Pricing> pricings { get; set; }
    }
}
