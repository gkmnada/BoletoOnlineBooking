using Catalog.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Entities
{
    public class MovieCrew : BaseEntity
    {
        [Key]
        public string CrewID { get; set; } = Guid.NewGuid().ToString("D");
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public Movie Movie { get; set; }
    }
}
