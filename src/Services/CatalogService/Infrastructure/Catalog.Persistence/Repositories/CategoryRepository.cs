using Catalog.Application.Interfaces.Repositories;
using Catalog.Domain.Entities;
using Catalog.Persistence.Context;

namespace Catalog.Persistence.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationContext _context;

        public CategoryRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
