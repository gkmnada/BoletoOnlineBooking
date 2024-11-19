using System.Linq.Expressions;

namespace Ticket.Application.Interfaces.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> ListAsync(CancellationToken cancellationToken);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);
        Task<List<T>> ListByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
        Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    }
}
