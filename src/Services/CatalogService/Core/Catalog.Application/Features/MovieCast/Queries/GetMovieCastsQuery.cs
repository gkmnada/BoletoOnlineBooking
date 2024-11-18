using Catalog.Application.Features.MovieCast.Results;
using MediatR;

namespace Catalog.Application.Features.MovieCast.Queries
{
    public class GetMovieCastsQuery : IRequest<List<GetMovieCastsQueryResult>>
    {
        public string MovieID { get; set; }

        public GetMovieCastsQuery(string id)
        {
            MovieID = id;
        }
    }
}
