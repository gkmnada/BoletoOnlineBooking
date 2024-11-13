using Catalog.Application.Features.Category.Validators;
using Catalog.Application.Features.Movie.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Catalog.Application.Common.Extensions
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

            services.AddScoped<CreateMovieValidator>();
            services.AddScoped<UpdateMovieValidator>();

            return services;
        }
    }
}
