﻿namespace Catalog.Application.Features.MovieCast.Results
{
    public class GetMovieCastsQueryResult
    {
        public string CastID { get; set; }
        public string CastName { get; set; }
        public string Character { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
