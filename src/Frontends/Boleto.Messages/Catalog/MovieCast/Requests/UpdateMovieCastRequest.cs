namespace Boleto.Messages.Catalog.MovieCast.Requests
{
    public class UpdateMovieCastRequest
    {
        public string CastID { get; set; }
        public string CastName { get; set; }
        public string Character { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
