using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieDetail.Requests;
using Boleto.Messages.Catalog.MovieDetail.Responses;

namespace Boleto.Business.Services.Catalog.MovieDetail
{
    public interface IMovieDetailService
    {
        Task<MovieDetailResponse> GetMovieDetailByMovieIdAsync(string id);
        Task<BaseResponse> CreateMovieDetailAsync(CreateMovieDetailRequest request);
        Task<BaseResponse> UpdateMovieDetailAsync(UpdateMovieDetailRequest request);
        Task<BaseResponse> DeleteMovieDetailAsync(string id);
        Task<MovieDetailResponse> GetMovieDetailAsync(string id);
    }
}
