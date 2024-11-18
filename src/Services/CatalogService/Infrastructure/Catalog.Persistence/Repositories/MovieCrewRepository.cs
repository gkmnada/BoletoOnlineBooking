using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.Context;

namespace Catalog.Persistence.Repositories
{
    public class MovieCrewRepository : GenericRepository<MovieCrew>, IMovieCrewRepository
    {
        private readonly ApplicationContext _context;

        public MovieCrewRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
