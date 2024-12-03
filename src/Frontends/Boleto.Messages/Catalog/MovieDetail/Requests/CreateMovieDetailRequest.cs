using Microsoft.AspNetCore.Http;

namespace Boleto.Messages.Catalog.MovieDetail.Requests
{
    public class CreateMovieDetailRequest
    {
        public IFormFile ImageURL { get; set; }
        public IFormFile VideoURL { get; set; }
        public string Description { get; set; }
        public string MovieID { get; set; }
    }
}
