using Ticket.Application.Interfaces.Repositories;
using Ticket.Domain.Entities;
using Ticket.Persistence.Context;

namespace Ticket.Persistence.Repositories
{
    public class CityRepository : GenericRepository<City>, ICityRepository
    {
        private readonly ApplicationContext _context;

        public CityRepository(ApplicationContext context) : base(context)
        {
            _context = context;
        }
    }
}
