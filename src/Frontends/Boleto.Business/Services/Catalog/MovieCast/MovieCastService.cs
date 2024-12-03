using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieCast.Requests;
using Boleto.Messages.Catalog.MovieCast.Responses;
using System.Net.Http.Json;

namespace Boleto.Business.Services.Catalog.MovieCast
{
    public class MovieCastService : IMovieCastService
    {
        private readonly HttpClient _httpClient;

        public MovieCastService(HttpClient httpClient)
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

        public async Task<BaseResponse> CreateMovieCastAsync(CreateMovieCastRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("catalog/moviecasts", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<BaseResponse> DeleteMovieCastAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"catalog/moviecasts/{id}");
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<MovieCastDetailResponse> GetMovieCastDetailAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/moviecasts/getmoviecast/{id}");
            return await HandleResponseAsync<MovieCastDetailResponse>(response);
        }

        public async Task<List<MovieCastResponse>> ListMovieCastsAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/moviecasts/listmoviecasts/{id}");
            return await HandleResponseAsync<List<MovieCastResponse>>(response);
        }

        public async Task<BaseResponse> UpdateMovieCastAsync(UpdateMovieCastRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("catalog/moviecasts", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }
    }
}
