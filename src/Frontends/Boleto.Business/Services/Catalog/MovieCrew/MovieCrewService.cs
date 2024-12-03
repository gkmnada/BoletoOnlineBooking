using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieCrew.Requests;
using Boleto.Messages.Catalog.MovieCrew.Responses;
using System.Net.Http.Json;

namespace Boleto.Business.Services.Catalog.MovieCrew
{
    public class MovieCrewService : IMovieCrewService
    {
        private readonly HttpClient _httpClient;

        public MovieCrewService(HttpClient httpClient)
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

        public async Task<BaseResponse> CreateMovieCrewAsync(CreateMovieCrewRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("catalog/moviecrews", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<BaseResponse> DeleteMovieCrewAsync(string id)
        {
            var response = await _httpClient.DeleteAsync($"catalog/moviecrews/{id}");
            return await HandleResponseAsync<BaseResponse>(response);
        }

        public async Task<MovieCrewDetailResponse> GetMovieCrewDetailAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/moviecrews/getmoviecrew/{id}");
            return await HandleResponseAsync<MovieCrewDetailResponse>(response);
        }

        public async Task<List<MovieCrewResponse>> ListMovieCrewsAsync(string id)
        {
            var response = await _httpClient.GetAsync($"catalog/moviecrews/listmoviecrews/{id}");
            return await HandleResponseAsync<List<MovieCrewResponse>>(response);
        }

        public async Task<BaseResponse> UpdateMovieCrewAsync(UpdateMovieCrewRequest request)
        {
            var response = await _httpClient.PutAsJsonAsync("catalog/moviecrews", request);
            return await HandleResponseAsync<BaseResponse>(response);
        }
    }
}
