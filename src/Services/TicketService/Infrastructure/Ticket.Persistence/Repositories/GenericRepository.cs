using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Ticket.Application.Interfaces.Repositories;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationContext _context;

        public GenericRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<T> GetByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(filter, cancellationToken);
        }

        public async Task<T> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().FindAsync(id, cancellationToken);
        }

        public async Task<List<T>> ListAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<T>().ToListAsync(cancellationToken);
        }

        public async Task<List<T>> ListByFilterAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
        {
            return await _context.Set<T>().Where(filter).ToListAsync(cancellationToken);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            await Task.CompletedTask;
        }
    }
}
