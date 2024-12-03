using Microsoft.AspNetCore.Http;

namespace Boleto.Messages.Catalog.MovieImage.Requests
{
    public class CreateMovieImageRequest
    {
        public IFormFile ImageURL { get; set; }
        public string MovieID { get; set; }
    }
}
