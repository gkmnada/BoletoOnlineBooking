using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieImage.Requests;
using Boleto.Messages.Catalog.MovieImage.Responses;

namespace Boleto.Business.Services.Catalog.MovieImage
{
    public interface IMovieImageService
    {
        Task<List<MovieImageResponse>> ListMovieImagesAsync(string id);
        Task<BaseResponse> CreateMovieImageAsync(CreateMovieImageRequest request);
        Task<BaseResponse> UpdateMovieImageAsync(UpdateMovieImageRequest request);
        Task<BaseResponse> DeleteMovieImageAsync(string id);
        Task<MovieImageDetailResponse> GetMovieImageDetailAsync(string id);
    }
}
