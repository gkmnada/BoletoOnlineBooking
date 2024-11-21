using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Ticket.Application.Features.Category.Validators;
using Ticket.Application.Features.Cinema.Validators;
using Ticket.Application.Features.City.Validators;
using Ticket.Application.Features.Hall.Validators;
using Ticket.Application.Features.MovieTicket.Validators;
using Ticket.Application.Features.Pricing.Validators;
using Ticket.Application.Features.Seat.Validators;
using Ticket.Application.Features.Session.Validators;

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

            services.AddScoped<CreateCinemaValidator>();
            services.AddScoped<UpdateCinemaValidator>();

            services.AddScoped<CreateCityValidator>();
            services.AddScoped<UpdateCityValidator>();

            services.AddScoped<CreateHallValidator>();
            services.AddScoped<UpdateHallValidator>();

            services.AddScoped<CreatePricingValidator>();
            services.AddScoped<UpdatePricingValidator>();

            services.AddScoped<CreateSeatValidator>();
            services.AddScoped<UpdateSeatValidator>();

            services.AddScoped<CreateSessionValidator>();
            services.AddScoped<UpdateSessionValidator>();

            services.AddScoped<CreateMovieTicketValidator>();

            return services;
        }
    }
}
