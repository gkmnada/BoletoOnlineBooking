using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class HallRepository : GenericRepository<Hall>, IHallRepository
    {
        private readonly ApplicationContext _context;

        public HallRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
