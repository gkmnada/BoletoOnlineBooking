using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class SessionRepository : GenericRepository<Session>, ISessionRepository
    {
        private readonly ApplicationContext _context;

        public SessionRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
