using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Hall : BaseEntity
    {
        public string name { get; set; }
        public int capacity { get; set; }
        public string cinema_id { get; set; }
        public Cinema cinema { get; set; }
        public List<Seat> seats { get; set; }
        public List<Session> sessions { get; set; }
    }
}
