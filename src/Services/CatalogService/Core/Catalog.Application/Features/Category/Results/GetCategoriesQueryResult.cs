namespace Catalog.Application.Features.Category.Results
{
    public class GetCategoriesQueryResult
    {
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public string SlugURL { get; set; }
        public bool IsActive { get; set; }
    }
}
