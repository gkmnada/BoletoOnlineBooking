using MassTransit;
using Microsoft.Extensions.Options;
using Search.API.Consumers;
using Search.API.Services;
using Search.API.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddScoped<ISearchService, SearchService>();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IDatabaseSettings>(options =>
{
    return options.GetRequiredService<IOptions<DatabaseSettings>>().Value;
});

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<MovieCreatedConsumer>();
    options.AddConsumer<MovieUpdatedConsumer>();
    options.AddConsumer<MovieDeletedConsumer>();

    options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));
    options.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        config.ReceiveEndpoint("search-movie-created", e =>
        {
            e.ConfigureConsumer<MovieCreatedConsumer>(context);
        });

        config.ReceiveEndpoint("search-movie-updated", e =>
        {
            e.ConfigureConsumer<MovieUpdatedConsumer>(context);
        });

        config.ReceiveEndpoint("search-movie-deleted", e =>
        {
            e.ConfigureConsumer<MovieDeletedConsumer>(context);
        });
    });
});

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
