using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.Movie.Commands
{
    public class DeleteMovieCommand : IRequest<BaseResponse>
    {
        public string MovieID { get; set; }

        public DeleteMovieCommand(string id)
        {
            MovieID = id;
        }
    }
}
