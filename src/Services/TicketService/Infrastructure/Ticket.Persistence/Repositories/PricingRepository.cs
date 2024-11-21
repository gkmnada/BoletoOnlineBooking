using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class PricingRepository : GenericRepository<Pricing>, IPricingRepository
    {
        private readonly ApplicationContext _context;

        public PricingRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
