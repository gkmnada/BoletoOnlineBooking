using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.MovieDetail.Commands
{
    public class DeleteMovieDetailCommand : IRequest<BaseResponse>
    {
        public string DetailID { get; set; }

        public DeleteMovieDetailCommand(string id)
        {
            DetailID = id;
        }
    }
}
