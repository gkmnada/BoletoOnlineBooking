using Catalog.Application.Interfaces.Repositories;
using Catalog.Application.Interfaces.Services;
using Catalog.Persistence.Repositories;
using Catalog.Persistence.Services;

namespace Catalog.API.Registrations
{
    public static class ServiceRegistration
    {
        public static void PresentationService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IMovieRepository, MovieRepository>();
            services.AddScoped<IMovieImageRepository, MovieImageRepository>();
            services.AddScoped<IMovieDetailRepository, MovieDetailRepository>();
            services.AddScoped<IMovieCastRepository, MovieCastRepository>();
            services.AddScoped<IMovieCrewRepository, MovieCrewRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IFileService, FileService>();
        }
    }
}
