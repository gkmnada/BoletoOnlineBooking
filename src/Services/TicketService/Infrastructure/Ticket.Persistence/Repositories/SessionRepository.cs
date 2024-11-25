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
            var values = await _context.Sessions
                .Include(x => x.Cinema)
                .Include(x => x.Hall)
                .FirstOrDefaultAsync(x => x.SessionID == id, cancellationToken);

            return values;
        }
    }
}
