using Ticket.Domain.Entities;

namespace Ticket.Application.Interfaces.Repositories
{
    public interface ISessionRepository : IGenericRepository<Session>
    {
        Task<Session> GetSessionAsync(string id, CancellationToken cancellationToken);
    }
}
