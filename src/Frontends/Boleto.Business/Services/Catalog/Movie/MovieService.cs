using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.Movie.Requests;
using Boleto.Messages.Catalog.Movie.Responses;
using System.Net.Http.Json;

namespace Boleto.Business.Services.Catalog.Movie
{
    public class MovieService : IMovieService
    {
        private readonly HttpClient _httpClient;

        public MovieService(HttpClient httpClient)
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

        public async Task<BaseResponse> CreateMovieAsync(CreateMovieRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("catalog/movies", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<BaseResponse> DeleteMovieAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"catalog/movies/{id}");
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<MovieDetailResponse> GetMovieDetailAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/movies/{id}");
            return await HandleResponseAsync<MovieDetailResponse>(response);
        }

        public async Task<List<MovieResponse>> ListMoviesAsync()
        {
            var response = await _httpClient.GetAsync("catalog/movies");
            return await HandleResponseAsync<List<MovieResponse>>(response);
        }

        public async Task<BaseResponse> UpdateMovieAsync(UpdateMovieRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("catalog/movies", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }
    }
}
