using Catalog.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Entities
{
    public class MovieDetail : BaseEntity
    {
        [Key]
        public string DetailID { get; set; } = Guid.NewGuid().ToString("D");
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string MovieID { get; set; }
        public Movie Movie { get; set; }
    }
}
