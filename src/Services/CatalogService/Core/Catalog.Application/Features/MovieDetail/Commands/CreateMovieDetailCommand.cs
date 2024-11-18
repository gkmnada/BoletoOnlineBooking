using Catalog.Application.Common.Base;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Catalog.Application.Features.MovieDetail.Commands
{
    public class CreateMovieDetailCommand : IRequest<BaseResponse>
    {
        public IFormFile ImageURL { get; set; }
        public IFormFile VideoURL { get; set; }
        public string Description { get; set; }
        public string MovieID { get; set; }
    }
}
