using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.Movie.Requests;
using Boleto.Messages.Catalog.Movie.Responses;

namespace Boleto.Business.Services.Catalog.Movie
{
    public interface IMovieService
    {
        Task<List<MovieResponse>> ListMoviesAsync();
        Task<BaseResponse> CreateMovieAsync(CreateMovieRequest request);
        Task<BaseResponse> UpdateMovieAsync(UpdateMovieRequest request);
        Task<BaseResponse> DeleteMovieAsync(string id);
        Task<MovieDetailResponse> GetMovieDetailAsync(string id);
    }
}
