using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ticket.Application.Features.Category.Validators;

namespace Ticket.Application.Common.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ApplicationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
            });

            services.AddScoped<CreateCategoryValidator>();
            services.AddScoped<UpdateCategoryValidator>();

            return services;
        }
    }
}
