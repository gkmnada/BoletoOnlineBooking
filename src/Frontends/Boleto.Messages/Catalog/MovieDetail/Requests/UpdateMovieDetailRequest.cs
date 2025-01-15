using Microsoft.AspNetCore.Http;

namespace Boleto.Messages.Catalog.MovieDetail.Requests
{
    public class UpdateMovieDetailRequest
    {
        public string DetailID { get; set; }
        public IFormFile? ImageURL { get; set; }
        public IFormFile? VideoURL { get; set; }
        public string? ExistingImageURL { get; set; }
        public string? ExistingVideoURL { get; set; }
        public string Description { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
