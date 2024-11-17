using Elastic.Clients.Elasticsearch;
using Filter.API.Consumers;
using Filter.API.Services;
using MassTransit;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

var settings = new ElasticsearchClientSettings(new Uri(builder.Configuration["Elasticsearch:Uri"] ?? ""))
    .DefaultIndex(builder.Configuration["Elasticsearch:Index"] ?? "");
var client = new ElasticsearchClient(settings);

builder.Services.AddSingleton(client);

builder.Services.AddScoped<IFilterService, FilterService>();

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

        config.ReceiveEndpoint("filter-movie-created", endpoint =>
        {
            endpoint.ConfigureConsumer<MovieCreatedConsumer>(context);
        });

        config.ReceiveEndpoint("filter-movie-updated", endpoint =>
        {
            endpoint.ConfigureConsumer<MovieUpdatedConsumer>(context);
        });

        config.ReceiveEndpoint("filter-movie-deleted", endpoint =>
        {
            endpoint.ConfigureConsumer<MovieDeletedConsumer>(context);
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
