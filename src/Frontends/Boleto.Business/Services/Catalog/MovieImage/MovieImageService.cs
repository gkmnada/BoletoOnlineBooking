using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieImage.Requests;
using Boleto.Messages.Catalog.MovieImage.Responses;
using System.Net.Http.Json;

namespace Boleto.Business.Services.Catalog.MovieImage
{
    public class MovieImageService : IMovieImageService
    {
        private readonly HttpClient _httpClient;

        public MovieImageService(HttpClient httpClient)
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

        public async Task<BaseResponse> CreateMovieImageAsync(CreateMovieImageRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("catalog/movieimages", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<BaseResponse> DeleteMovieImageAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"catalog/movieimages/{id}");
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<List<MovieImageResponse>> ListMovieImagesAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/movieimages/listmovieimages/{id}");
            return await HandleResponseAsync<List<MovieImageResponse>>(response);
        }

        public async Task<MovieImageDetailResponse> GetMovieImageDetailAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/movieimages/getmovieimage/{id}");
            return await HandleResponseAsync<MovieImageDetailResponse>(response);
        }

        public async Task<BaseResponse> UpdateMovieImageAsync(UpdateMovieImageRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("catalog/movieimages", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }
    }
}
