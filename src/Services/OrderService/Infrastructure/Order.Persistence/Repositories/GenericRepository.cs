using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces.Repositories;
using Order.Persistence.Context;
using System.Linq.Expressions;

namespace Order.Persistence.Repositories
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
            return await _context.Set<T>().FindAsync(id);
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
            _context.Update(entity);
            await Task.CompletedTask;
        }
    }
}
