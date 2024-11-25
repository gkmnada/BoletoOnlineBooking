using Ticket.Application.Interfaces.Repositories;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class TicketRepository : GenericRepository<Domain.Entities.Ticket>, ITicketRepository
    {
        private readonly ApplicationContext _context;

        public TicketRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
