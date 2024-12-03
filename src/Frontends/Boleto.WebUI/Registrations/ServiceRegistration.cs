using Boleto.Business.Services.Catalog.Category;
using Boleto.Business.Services.Catalog.Movie;
using Boleto.Business.Services.Catalog.MovieCast;
using Boleto.Business.Services.Catalog.MovieCrew;
using Boleto.Business.Services.Catalog.MovieDetail;
using Boleto.Business.Services.Catalog.MovieImage;
using Boleto.WebUI.Handlers;
using Boleto.WebUI.Services.IdentityServices;
using Boleto.WebUI.Settings;

namespace Boleto.WebUI.Registrations
{
    public static class ServiceRegistration
    {
        public static void RegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ClientSettings>(configuration.GetSection("ClientSettings"));
            services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));

            services.AddScoped<ResourceOwnerPasswordTokenHandler>();
            services.AddScoped<IIdentityService, IdentityService>();

            var values = configuration.GetSection("ApiSettings").Get<ApiSettings>();

            services.AddHttpClient<ICategoryService, CategoryService>(options =>
            {
                options.BaseAddress = new Uri(values.GatewayUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IMovieService, MovieService>(options =>
            {
                options.BaseAddress = new Uri(values.GatewayUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IMovieCastService, MovieCastService>(options =>
            {
                options.BaseAddress = new Uri(values.GatewayUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IMovieCrewService, MovieCrewService>(options =>
            {
                options.BaseAddress = new Uri(values.GatewayUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IMovieDetailService, MovieDetailService>(options =>
            {
                options.BaseAddress = new Uri(values.GatewayUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();

            services.AddHttpClient<IMovieImageService, MovieImageService>(options =>
            {
                options.BaseAddress = new Uri(values.GatewayUri);
            }).AddHttpMessageHandler<ResourceOwnerPasswordTokenHandler>();
        }
    }
}
