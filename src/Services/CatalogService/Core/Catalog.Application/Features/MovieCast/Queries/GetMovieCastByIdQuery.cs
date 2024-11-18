using Catalog.Application.Features.MovieCast.Results;
using MediatR;

namespace Catalog.Application.Features.MovieCast.Queries
{
    public class GetMovieCastByIdQuery : IRequest<GetMovieCastByIdQueryResult>
    {
        public string CastID { get; set; }

        public GetMovieCastByIdQuery(string id)
        {
            CastID = id;
        }
    }
}
