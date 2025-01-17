﻿namespace Boleto.Messages.Catalog.MovieCast.Requests
{
    public class CreateMovieCastRequest
    {
        public List<CreateMovieCastItem> MovieCasts { get; set; }
    }

    public class CreateMovieCastItem
    {
        public string CastName { get; set; }
        public string Character { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
    }
}
