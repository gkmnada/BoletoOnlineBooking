using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.MovieCrew.Commands
{
    public class DeleteMovieCrewCommand : IRequest<BaseResponse>
    {
        public string CrewID { get; set; }

        public DeleteMovieCrewCommand(string id)
        {
            CrewID = id;
        }
    }
}
