namespace Boleto.Messages.Catalog.MovieCrew.Requests
{
    public class UpdateMovieCrewRequest
    {
        public string CrewID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
