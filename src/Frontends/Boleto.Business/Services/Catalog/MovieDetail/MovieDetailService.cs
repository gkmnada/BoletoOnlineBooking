using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieDetail.Requests;
using Boleto.Messages.Catalog.MovieDetail.Responses;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    return new T();
                }

                var content = await response.Content.ReadAsStringAsync();

                if (string.IsNullOrEmpty(content))
                {
                    return new T();
                }

                return JsonConvert.DeserializeObject<T>(content) ?? new T();
            }
            return new T();
        }

        public async Task<BaseResponse> CreateMovieDetailAsync(CreateMovieDetailRequest request)
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(request.Description), "Description");
                content.Add(new StringContent(request.MovieID), "MovieID");

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

                if (request.VideoURL != null && request.VideoURL.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await request.VideoURL.CopyToAsync(stream);

                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.VideoURL.ContentType);

                        content.Add(fileContent, "VideoURL", request.VideoURL.FileName);
                    }
                }

                var response = await _httpClient.PostAsync("catalog/moviedetails", content);
                return await HandleResponseAsync<BaseResponse>(response);
            }
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
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(request.DetailID), "DetailID");
                content.Add(new StringContent(request.Description), "Description");
                content.Add(new StringContent(request.MovieID), "MovieID");
                content.Add(new StringContent(request.IsActive.ToString()), "IsActive");

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
                else if (!string.IsNullOrEmpty(request.ExistingImageURL))
                {
                    content.Add(new StringContent(request.ExistingImageURL), "ExistingImageURL");
                }

                if (request.VideoURL != null && request.VideoURL.Length > 0)
                {
                    using (var stream = new MemoryStream())
                    {
                        await request.VideoURL.CopyToAsync(stream);

                        var fileContent = new ByteArrayContent(stream.ToArray());
                        fileContent.Headers.ContentType = new MediaTypeHeaderValue(request.VideoURL.ContentType);

                        content.Add(fileContent, "VideoURL", request.VideoURL.FileName);
                    }
                }
                else if (!string.IsNullOrEmpty(request.ExistingVideoURL))
                {
                    content.Add(new StringContent(request.ExistingVideoURL), "ExistingVideoURL");
                }

                var response = await _httpClient.PutAsync("catalog/moviedetails", content);
                return await HandleResponseAsync<BaseResponse>(response);
            }
        }
    }
}
