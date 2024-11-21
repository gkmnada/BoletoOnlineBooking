using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class CinemaRepository : GenericRepository<Cinema>, ICinemaRepository
    {
        private readonly ApplicationContext _context;

        public CinemaRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
