using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Repositories
{
    public class MovieImageRepository : GenericRepository<MovieImage>, IMovieImageRepository
    {
        private readonly ApplicationContext _context;

        public MovieImageRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<MovieImage>> ListMovieImagesAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.MovieImages.Where(x => x.MovieID == id).ToListAsync(cancellationToken);
        }
    }
}
