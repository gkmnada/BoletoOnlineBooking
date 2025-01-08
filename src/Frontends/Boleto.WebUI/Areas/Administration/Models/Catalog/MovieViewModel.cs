using Boleto.Messages.Catalog.Movie.Requests;
using Boleto.Messages.Catalog.Movie.Responses;

namespace Boleto.WebUI.Areas.Administration.Models.Catalog
{
    public class MovieViewModel
    {
        public MovieDetailResponse MovieDetailResponse { get; set; }
        public UpdateMovieRequest UpdateMovieRequest { get; set; }
    }
}
