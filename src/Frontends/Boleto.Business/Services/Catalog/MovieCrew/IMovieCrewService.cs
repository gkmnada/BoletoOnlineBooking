using Boleto.Business.Common.Base;
using Boleto.Messages.Catalog.MovieCrew.Requests;
using Boleto.Messages.Catalog.MovieCrew.Responses;

namespace Boleto.Business.Services.Catalog.MovieCrew
{
    public interface IMovieCrewService
    {
        Task<List<MovieCrewResponse>> ListMovieCrewsAsync(string id);
        Task<BaseResponse> CreateMovieCrewAsync(CreateMovieCrewRequest request);
        Task<BaseResponse> UpdateMovieCrewAsync(UpdateMovieCrewRequest request);
        Task<BaseResponse> DeleteMovieCrewAsync(string id);
        Task<MovieCrewDetailResponse> GetMovieCrewDetailAsync(string id);
    }
}
