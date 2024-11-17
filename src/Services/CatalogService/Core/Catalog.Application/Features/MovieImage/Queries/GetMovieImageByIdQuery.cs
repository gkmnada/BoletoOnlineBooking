using Catalog.Application.Features.MovieImage.Results;
using MediatR;

namespace Catalog.Application.Features.MovieImage.Queries
{
    public class GetMovieImageByIdQuery : IRequest<GetMovieImageByIdQueryResult>
    {
        public string ImageID { get; set; }

        public GetMovieImageByIdQuery(string id)
        {
            ImageID = id;
        }
    }
}
