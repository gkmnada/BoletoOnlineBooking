namespace Boleto.Messages.Catalog.Category.Requests
{
    public class UpdateCategoryRequest
    {
        public string CategoryID { get; set; }
        public string Name { get; set; }
        public string SlugURL { get; set; }
        public bool IsActive { get; set; }
    }
}
