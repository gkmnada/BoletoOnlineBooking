using Microsoft.EntityFrameworkCore;
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

        public async Task<Session> GetSessionAsync(string id, CancellationToken cancellationToken)
        {
            var values = await _context.sessions
                .Include(x => x.cinema)
                .Include(x => x.hall)
                .FirstOrDefaultAsync(x => x.id == id, cancellationToken);

            return values;
        }
    }
}
