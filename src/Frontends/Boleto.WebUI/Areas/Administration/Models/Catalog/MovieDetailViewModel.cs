using Boleto.Messages.Catalog.MovieDetail.Requests;
using Boleto.Messages.Catalog.MovieDetail.Responses;

namespace Boleto.WebUI.Areas.Administration.Models.Catalog
{
    public class MovieDetailViewModel
    {
        public MovieDetailResponse MovieDetailResponse { get; set; }
        public UpdateMovieDetailRequest UpdateMovieDetailRequest { get; set; }
    }
}