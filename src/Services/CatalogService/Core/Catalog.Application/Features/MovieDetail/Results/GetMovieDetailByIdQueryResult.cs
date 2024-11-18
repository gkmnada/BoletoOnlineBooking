namespace Catalog.Application.Features.MovieDetail.Results
{
    public class GetMovieDetailByIdQueryResult
    {
        public string DetailID { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string Description { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
