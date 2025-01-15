using Catalog.Application.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Features.MovieDetail.Commands
{
    public class UpdateMovieDetailCommand : IRequest<BaseResponse>
    {
        public string DetailID { get; set; }
        public IFormFile? ImageURL { get; set; }
        public IFormFile? VideoURL { get; set; }
        public string? ExistingImageURL { get; set; }
        public string? ExistingVideoURL { get; set; }
        public string Description { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
