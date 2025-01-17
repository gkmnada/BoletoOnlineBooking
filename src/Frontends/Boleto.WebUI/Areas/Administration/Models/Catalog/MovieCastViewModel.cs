using Boleto.Messages.Catalog.MovieCast.Requests;
using Boleto.Messages.Catalog.MovieCast.Responses;

namespace Boleto.WebUI.Areas.Administration.Models.Catalog
{
    public class MovieCastViewModel
    {
        public MovieCastDetailResponse MovieCastDetailResponse { get; set; }
        public UpdateMovieCastRequest UpdateMovieCastRequest { get; set; }
    }
}
