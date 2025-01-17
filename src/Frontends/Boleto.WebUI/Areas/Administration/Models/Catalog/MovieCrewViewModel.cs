using Boleto.Messages.Catalog.MovieCrew.Requests;
using Boleto.Messages.Catalog.MovieCrew.Responses;

namespace Boleto.WebUI.Areas.Administration.Models.Catalog
{
    public class MovieCrewViewModel
    {
        public MovieCrewDetailResponse MovieCrewDetailResponse { get; set; }
        public UpdateMovieCrewRequest UpdateMovieCrewRequest { get; set; }
    }
}
