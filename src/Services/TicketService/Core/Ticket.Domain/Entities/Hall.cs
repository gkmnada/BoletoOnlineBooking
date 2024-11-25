using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Hall : BaseEntity
    {
        [Key]
        public string HallID { get; set; } = Guid.NewGuid().ToString("D");
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string CinemaID { get; set; }
        public Cinema Cinema { get; set; }
        public List<Seat> Seats { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
