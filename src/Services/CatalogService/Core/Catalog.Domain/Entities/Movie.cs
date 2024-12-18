﻿using Catalog.Domain.Common.Base;
using System.ComponentModel.DataAnnotations;

namespace Catalog.Domain.Entities
{
    public class Movie : BaseEntity
    {
        [Key]
        public string MovieID { get; set; } = Guid.NewGuid().ToString("D");
        public string MovieName { get; set; }
        public List<string> Genre { get; set; }
        public List<string> Language { get; set; }
        public string Duration { get; set; }
        public string ReleaseDate { get; set; }
        public int Rating { get; set; }
        public int AudienceScore { get; set; }
        public string ImageURL { get; set; }
        public string SlugURL { get; set; }
        public string CategoryID { get; set; }
        public Category Category { get; set; }
        public List<MovieImage> MovieImages { get; set; }
        public List<MovieDetail> MovieDetails { get; set; }
        public List<MovieCast> MovieCasts { get; set; }
        public List<MovieCrew> MovieCrews { get; set; }
    }
}
