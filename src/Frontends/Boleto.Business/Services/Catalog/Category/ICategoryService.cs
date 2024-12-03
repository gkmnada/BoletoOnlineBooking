using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.Category.Requests;
using Boleto.Messages.Catalog.Category.Responses;

namespace Boleto.Business.Services.Catalog.Category
{
    public interface ICategoryService
    {
        Task<List<CategoryResponse>> ListCategoriesAsync();
        Task<BaseResponse> CreateCategoryAsync(CreateCategoryRequest request);
        Task<BaseResponse> UpdateCategoryAsync(UpdateCategoryRequest request);
        Task<BaseResponse> DeleteCategoryAsync(string id);
        Task<CategoryDetailResponse> GetCategoryDetailAsync(string id);
    }
}
