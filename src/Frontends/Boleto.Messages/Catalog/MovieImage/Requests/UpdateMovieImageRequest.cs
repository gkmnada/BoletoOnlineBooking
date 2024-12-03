using Microsoft.AspNetCore.Http;

namespace Boleto.Messages.Catalog.MovieImage.Requests
{
    public class UpdateMovieImageRequest
    {
        public string ImageID { get; set; }
        public IFormFile ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
