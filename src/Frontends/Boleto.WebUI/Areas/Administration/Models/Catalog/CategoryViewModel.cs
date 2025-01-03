using Boleto.Messages.Catalog.Category.Requests;
using Boleto.Messages.Catalog.Category.Responses;

namespace Boleto.WebUI.Areas.Administration.Models.Catalog
{
    public class CategoryViewModel
    {
        public CategoryDetailResponse CategoryDetailResponse { get; set; }
        public UpdateCategoryRequest UpdateCategoryRequest { get; set; }
    }
}
