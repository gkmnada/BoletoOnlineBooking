using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Seat : BaseEntity
    {
        [Key]
        public string SeatID { get; set; } = Guid.NewGuid().ToString("D");
        public string Row { get; set; }
        public int Number { get; set; }
        public string HallID { get; set; }
        public Hall Hall { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
