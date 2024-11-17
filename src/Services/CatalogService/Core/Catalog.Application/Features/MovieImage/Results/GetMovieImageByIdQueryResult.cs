namespace Catalog.Application.Features.MovieImage.Results
{
    public class GetMovieImageByIdQueryResult
    {
        public string ImageID { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
