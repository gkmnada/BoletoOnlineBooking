using Catalog.Domain.Common.Base;

namespace Catalog.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryID { get; set; } = Guid.NewGuid().ToString("D");
        public string Name { get; set; }
        public string SlugURL { get; set; }
        public List<Movie> Movies { get; set; }
    }
}
