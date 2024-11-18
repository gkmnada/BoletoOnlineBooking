using Catalog.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Entities
{
    public class MovieCast : BaseEntity
    {
        [Key]
        public string CastID { get; set; } = Guid.NewGuid().ToString("D");
        public string CastName { get; set; }
        public string Character { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public Movie Movie { get; set; }
    }
}
