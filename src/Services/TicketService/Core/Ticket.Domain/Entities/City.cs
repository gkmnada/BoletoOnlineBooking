using System.ComponentModel.DataAnnotations;

namespace Ticket.Domain.Entities
{
    public class City
    {
        [Key]
        public int city_id { get; set; }
        public string name { get; set; }
        public List<Cinema> cinemas { get; set; }
    }
}
