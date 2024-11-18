using Catalog.Application.Features.MovieCrew.Results;
using MediatR;

namespace Catalog.Application.Features.MovieCrew.Queries
{
    public class GetMovieCrewByIdQuery : IRequest<GetMovieCrewByIdQueryResult>
    {
        public string CrewID { get; set; }

        public GetMovieCrewByIdQuery(string id)
        {
            CrewID = id;
        }
    }
}
