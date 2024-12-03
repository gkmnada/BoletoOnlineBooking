using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieCast.Requests;
using Boleto.Messages.Catalog.MovieCast.Responses;

namespace Boleto.Business.Services.Catalog.MovieCast
{
    public interface IMovieCastService
    {
        Task<List<MovieCastResponse>> ListMovieCastsAsync(string id);
        Task<BaseResponse> CreateMovieCastAsync(CreateMovieCastRequest request);
        Task<BaseResponse> UpdateMovieCastAsync(UpdateMovieCastRequest request);
        Task<BaseResponse> DeleteMovieCastAsync(string id);
        Task<MovieCastDetailResponse> GetMovieCastDetailAsync(string id);
    }
}
