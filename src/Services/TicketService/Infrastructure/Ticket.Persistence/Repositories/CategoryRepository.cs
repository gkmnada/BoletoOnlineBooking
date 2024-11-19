using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
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
