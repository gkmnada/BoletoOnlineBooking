using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.MovieCast.Commands
{
    public class DeleteMovieCastCommand : IRequest<BaseResponse>
    {
        public string CastID { get; set; }

        public DeleteMovieCastCommand(string id)
        {
            CastID = id;
        }
    }
}
