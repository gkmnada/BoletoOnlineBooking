using Catalog.Application.Features.MovieImage.Results;
using MediatR;

namespace Catalog.Application.Features.MovieImage.Queries
{
    public class GetMovieImagesQuery : IRequest<List<GetMovieImagesQueryResult>>
    {
        public string MovieID { get; set; }

        public GetMovieImagesQuery(string id)
        {
            MovieID = id;
        }
    }
}
