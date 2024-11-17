using Catalog.Application.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Features.MovieImage.Commands
{
    public class CreateMovieImageCommand : IRequest<BaseResponse>
    {
        public IFormFile ImageURL { get; set; }
        public string MovieID { get; set; }
    }
}
