using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.MovieImage.Commands
{
    public class DeleteMovieImageCommand : IRequest<BaseResponse>
    {
        public string ImageID { get; set; }

        public DeleteMovieImageCommand(string id)
        {
            ImageID = id;
        }
    }
}
