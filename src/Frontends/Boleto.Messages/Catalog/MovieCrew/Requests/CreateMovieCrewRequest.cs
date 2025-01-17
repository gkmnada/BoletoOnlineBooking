namespace Boleto.Messages.Catalog.MovieCrew.Requests
{
    public class CreateMovieCrewRequest
    {
        public List<CreateMovieCrewItem> MovieCrew { get; set; }
    }

    public class CreateMovieCrewItem
    {
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
    }
}
