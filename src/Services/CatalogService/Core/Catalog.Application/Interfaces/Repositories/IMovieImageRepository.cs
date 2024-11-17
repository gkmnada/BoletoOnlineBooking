using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces.Repositories
{
    public interface IMovieImageRepository : IGenericRepository<MovieImage>
    {
        Task<List<MovieImage>> ListMovieImagesAsync(string id, CancellationToken cancellationToken);
    }
}
