using Catalog.Application.Common.Base;
using MediatR;

namespace Catalog.Application.Features.MovieCrew.Commands
{
    public class UpdateMovieCrewCommand : IRequest<BaseResponse>
    {
        public string CrewID { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string ImageURL { get; set; }
        public string MovieID { get; set; }
        public bool IsActive { get; set; }
    }
}
