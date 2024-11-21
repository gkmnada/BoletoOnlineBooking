using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Cinema : BaseEntity
    {
        public string name { get; set; }
        public string address { get; set; }
        public string city_id { get; set; }
        public City city { get; set; }
        public List<Hall> halls { get; set; }
        public List<Session> sessions { get; set; }
    }
}
