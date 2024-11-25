using System.ComponentModel.DataAnnotations;
using Ticket.Domain.Common.Base;

namespace Ticket.Domain.Entities
{
    public class City : BaseEntity
    {
        [Key]
        public string CityID { get; set; } = Guid.NewGuid().ToString("D");
        public string Name { get; set; }
        public List<Cinema> Cinemas { get; set; }
    }
}
