using Catalog.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Entities
{
    public class Category : BaseEntity
    {
        [Key]
        public string CategoryID { get; set; } = Guid.NewGuid().ToString("D");
        public string Name { get; set; }
        public string SlugURL { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
