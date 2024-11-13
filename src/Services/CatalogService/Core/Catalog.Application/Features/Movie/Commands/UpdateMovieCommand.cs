using Catalog.Application.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Features.Movie.Commands
{
    public class UpdateMovieCommand : IRequest<BaseResponse>
    {
        public string MovieID { get; set; }
        public string MovieName { get; set; }
        public string Genre { get; set; }
        public string Language { get; set; }
        public string Duration { get; set; }
        public string ReleaseDate { get; set; }
        public int Rating { get; set; }
        public int AudienceScore { get; set; }
        public IFormFile ImageURL { get; set; }
        public IFormFile VideoURL { get; set; }
        public string SlugURL { get; set; }
        public string CategoryID { get; set; }
        public bool IsActive { get; set; }
    }
}
