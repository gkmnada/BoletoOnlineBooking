using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Seat : BaseEntity
    {
        public string row { get; set; }
        public int number { get; set; }
        public string hall_id { get; set; }
        public Hall hall { get; set; }
        public List<MovieTicket> movie_tickets { get; set; }
    }
}
