using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class SeatRepository : GenericRepository<Seat>, ISeatRepository
    {
        private readonly ApplicationContext _context;

        public SeatRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
