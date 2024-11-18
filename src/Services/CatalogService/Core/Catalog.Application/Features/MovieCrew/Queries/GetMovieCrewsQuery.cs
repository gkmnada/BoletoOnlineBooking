using Catalog.Application.Features.MovieCrew.Results;
using MediatR;

namespace Catalog.Application.Features.MovieCrew.Queries
{
    public class GetMovieCrewsQuery : IRequest<List<GetMovieCrewsQueryResult>>
    {
        public string MovieID { get; set; }

        public GetMovieCrewsQuery(string id)
        {
            MovieID = id;
        }
    }
}
