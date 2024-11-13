using Catalog.Application.Features.Movie.Results;
using MediatR;

namespace Catalog.Application.Features.Movie.Queries
{
    public class GetMoviesQuery : IRequest<List<GetMoviesQueryResult>>
    {
    }
}
