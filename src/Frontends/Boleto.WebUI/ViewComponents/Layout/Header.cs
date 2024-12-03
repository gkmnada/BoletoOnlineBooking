using Boleto.Business.Services.Catalog.Category;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.ViewComponents.Layout
{
    public class Header : ViewComponent
    {
        private readonly ICategoryService _categoryService;

        public Header(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var response = await _categoryService.ListCategoriesAsync();
            return View(response);
        }
    }
}
