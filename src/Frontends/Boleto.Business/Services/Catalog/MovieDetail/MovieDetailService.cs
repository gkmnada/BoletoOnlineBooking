using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieDetail.Requests;
using Boleto.Messages.Catalog.MovieDetail.Responses;
using System.Net.Http.Json;

namespace Boleto.Business.Services.Catalog.MovieDetail
{
    public class MovieDetailService : IMovieDetailService
    {
        private readonly HttpClient _httpClient;

        public MovieDetailService(HttpClient httpClient)
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

        public async Task<BaseResponse> CreateMovieDetailAsync(CreateMovieDetailRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("catalog/moviedetails", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<BaseResponse> DeleteMovieDetailAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"catalog/moviedetails/{id}");
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<MovieDetailResponse> GetMovieDetailAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/moviedetails/getmoviedetail/{id}");
            return await HandleResponseAsync<MovieDetailResponse>(response);
        }

        public async Task<MovieDetailResponse> GetMovieDetailByMovieIdAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/moviedetails/{id}");
            return await HandleResponseAsync<MovieDetailResponse>(response);
        }

        public async Task<BaseResponse> UpdateMovieDetailAsync(UpdateMovieDetailRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("catalog/moviedetails", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }
    }
}
