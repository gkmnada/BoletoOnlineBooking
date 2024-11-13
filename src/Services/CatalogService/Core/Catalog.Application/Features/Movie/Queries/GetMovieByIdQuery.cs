using Catalog.Application.Features.Movie.Results;
using MediatR;

namespace Catalog.Application.Features.Movie.Queries
{
    public class GetMovieByIdQuery : IRequest<GetMovieByIdQueryResult>
    {
        public string MovieID { get; set; }

        public GetMovieByIdQuery(string id)
        {
            MovieID = id;
        }
    }
}
