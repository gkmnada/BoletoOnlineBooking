using Catalog.Application.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Features.MovieImage.Commands
{
    public class UpdateMovieImageCommand : IRequest<BaseResponse>
    {
        public string ImageID { get; set; }
        public IFormFile ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
