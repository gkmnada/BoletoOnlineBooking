using Catalog.Application.Features.MovieDetail.Results;
using MediatR;

namespace Catalog.Application.Features.MovieDetail.Queries
{
    public class GetMovieDetailByIdQuery : IRequest<GetMovieDetailByIdQueryResult>
    {
        public string DetailID { get; set; }

        public GetMovieDetailByIdQuery(string id)
        {
            DetailID = id;
        }
    }
}
