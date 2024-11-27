using Order.Application.Interfaces.Repositories;
using Order.Domain.Entities;
using Order.Persistence.Context;

namespace Order.Persistence.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationContext _context;

        public OrderDetailRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
