using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.Category.Requests;
using Boleto.Messages.Catalog.Category.Responses;
using System.Net.Http.Json;

namespace Boleto.Business.Services.Catalog.Category
{
    public class CategoryService : ICategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) where T : new()
        {
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<T>() ?? new T();
            }

            return new T();
        }

        public async Task<BaseResponse> CreateCategoryAsync(CreateCategoryRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("catalog/categories", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<BaseResponse> DeleteCategoryAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"catalog/categories/{id}");
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<CategoryDetailResponse> GetCategoryDetailAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/categories/{id}");
            return await HandleResponseAsync<CategoryDetailResponse>(response);
        }

        public async Task<List<CategoryResponse>> ListCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("catalog/categories");
            return await HandleResponseAsync<List<CategoryResponse>>(response);
        }

        public async Task<BaseResponse> UpdateCategoryAsync(UpdateCategoryRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("catalog/categories", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }
    }
}
