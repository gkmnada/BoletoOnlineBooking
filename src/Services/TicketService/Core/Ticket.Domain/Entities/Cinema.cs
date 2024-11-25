using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class Cinema : BaseEntity
    {
        [Key]
        public string CinemaID { get; set; } = Guid.NewGuid().ToString("D");
        public string Name { get; set; }
        public string Address { get; set; }
        public string CityID { get; set; }
        public City City { get; set; }
        public List<Hall> Halls { get; set; }
        public List<Session> Sessions { get; set; }
    }
}
