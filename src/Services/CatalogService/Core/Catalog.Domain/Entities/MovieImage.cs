using Catalog.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Entities
{
    public class MovieImage : BaseEntity
    {
        [Key]
        public string ImageID { get; set; } = Guid.NewGuid().ToString("D");
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public Movie Movie { get; set; }
    }
}
