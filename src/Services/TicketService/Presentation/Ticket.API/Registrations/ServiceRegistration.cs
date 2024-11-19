using Ticket.Application.Interfaces.Repositories;
using Ticket.Persistence.Repositories;

namespace Ticket.API.Registrations
{
    public static class ServiceRegistration
    {
        public static void PresentationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
