using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Session : BaseEntity
    {
        [Key]
        public string SessionID { get; set; } = Guid.NewGuid().ToString("D");
        public DateOnly SessionDate { get; set; }
        public TimeOnly SessionTime { get; set; }
        public string HallID { get; set; }
        public Hall Hall { get; set; }
        public string CinemaID { get; set; }
        public Cinema Cinema { get; set; }
        public string MovieID { get; set; }
        public List<Ticket> Tickets { get; set; }
        public List<Pricing> Pricings { get; set; }
    }
}
