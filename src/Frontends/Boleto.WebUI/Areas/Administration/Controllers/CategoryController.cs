using Boleto.Business.Services.Catalog.Category;
using Boleto.Business.Validators.Catalog.Category;
using Boleto.Messages.Catalog.Category.Requests;
using Boleto.WebUI.Areas.Administration.Models.Catalog;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Boleto.WebUI.Areas.Administration.Controllers
{
    [Authorize]
    [Area("Administration")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var values = await _categoryService.ListCategoriesAsync();
            return View(values);
        }

        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            var validator = new CreateCategoryValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(request);
            }

            await _categoryService.CreateCategoryAsync(request);
            return RedirectToAction("Index", "Category", new { area = "Administration" });
        }

        [Route("Administration/Category/UpdateCategory/{id}")]
        [HttpGet]
        public async Task<IActionResult> UpdateCategory(string id)
        {
            var response = await _categoryService.GetCategoryDetailAsync(id);

            ViewBag.IsActive = response.IsActive;

            var model = new CategoryViewModel
            {
                CategoryDetailResponse = response
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateCategory(CategoryViewModel model)
        {
            var validator = new UpdateCategoryValidator();
            var validationResult = await validator.ValidateAsync(model.UpdateCategoryRequest);

            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(model.UpdateCategoryRequest);
            }

            await _categoryService.UpdateCategoryAsync(model.UpdateCategoryRequest);
            return RedirectToAction("Index", "Category", new { area = "Administration" });
        }

        [Route("Administration/Category/DeleteCategory/{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategoryAsync(id);
            return RedirectToAction("Index", "Category", new { area = "Administration" });
        }
    }
}
