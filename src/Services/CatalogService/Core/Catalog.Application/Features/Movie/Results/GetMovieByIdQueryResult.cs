﻿namespace Catalog.Application.Features.Movie.Results
{
    public class GetMovieByIdQueryResult
    {
        public string MovieID { get; set; }
        public string MovieName { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public string ReleaseDate { get; set; }
        public int Rating { get; set; }
        public int AudienceScore { get; set; }
        public string ImageURL { get; set; }
        public string VideoURL { get; set; }
        public string SlugURL { get; set; }
        public string CategoryID { get; set; }
        public bool IsActive { get; set; }
    }
}
