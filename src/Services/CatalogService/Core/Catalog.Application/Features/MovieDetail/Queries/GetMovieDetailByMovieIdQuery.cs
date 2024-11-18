using Catalog.Application.Features.MovieDetail.Results;
using MediatR;

namespace Catalog.Application.Features.MovieDetail.Queries
{
    public class GetMovieDetailByMovieIdQuery : IRequest<GetMovieDetailByMovieIdQueryResult>
    {
        public string MovieID { get; set; }

        public GetMovieDetailByMovieIdQuery(string id)
        {
            MovieID = id;
        }
    }
}
