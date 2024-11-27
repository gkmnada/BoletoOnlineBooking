using Order.Application.Interfaces.Repositories;
using Order.Persistence.Context;

namespace Order.Persistence.Repositories
{
    public class OrderRepository : GenericRepository<Domain.Entities.Order>, IOrderRepository
    {
        private readonly ApplicationContext _context;

        public OrderRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
