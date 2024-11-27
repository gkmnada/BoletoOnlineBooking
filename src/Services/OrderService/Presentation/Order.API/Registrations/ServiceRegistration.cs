using Order.Application.Interfaces.Repositories;
using Order.Persistence.Repositories;

namespace Order.API.Registrations
{
    public static class ServiceRegistration
    {
        public static void PresentationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
