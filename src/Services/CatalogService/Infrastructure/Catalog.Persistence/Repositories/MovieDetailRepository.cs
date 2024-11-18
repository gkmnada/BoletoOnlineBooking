using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.Context;

namespace Catalog.Persistence.Repositories
{
    public class MovieDetailRepository : GenericRepository<MovieDetail>, IMovieDetailRepository
    {
        private readonly ApplicationContext _context;

        public MovieDetailRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
