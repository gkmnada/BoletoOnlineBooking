using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class MovieTicketRepository : GenericRepository<MovieTicket>, IMovieTicketRepository
    {
        private readonly ApplicationContext _context;

        public MovieTicketRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
