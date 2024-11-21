using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class City : BaseEntity
    {
        public string name { get; set; }
        public List<Cinema> cinemas { get; set; }
    }
}
