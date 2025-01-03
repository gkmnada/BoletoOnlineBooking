using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.Movie.Requests;
using Boleto.Messages.Catalog.Movie.Responses;
using System.Net.Http.Headers;
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
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(request.MovieName), "MovieName");
                content.Add(new StringContent(request.SlugURL), "SlugURL");
                content.Add(new StringContent(request.Duration), "Duration");
                content.Add(new StringContent(request.ReleaseDate), "ReleaseDate");
                content.Add(new StringContent(request.Rating.ToString()), "Rating");
                content.Add(new StringContent(request.AudienceScore.ToString()), "AudienceScore");
                content.Add(new StringContent(request.CategoryID), "CategoryID");

                foreach (var genre in request.Genre)
                {
                    content.Add(new StringContent(genre), "Genre");
                }

                foreach (var language in request.Language)
                {
                    content.Add(new StringContent(language), "Language");
                }

                if (request.ImageURL != null && request.ImageURL.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await request.ImageURL.CopyToAsync(stream);

                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.ImageURL.ContentType);

                        content.Add(fileContent, "ImageURL", request.ImageURL.FileName);
                    }
                }

                var response = await _httpClient.PostAsync("catalog/movies", content);
                return await HandleResponseAsync<BaseResponse>(response);
            }
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
