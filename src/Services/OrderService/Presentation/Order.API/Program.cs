using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Order.API.Registrations;
using Order.API.Services;
using Order.Application.Common.Extensions;
using Order.Application.Consumers;
using Order.Application.Interfaces.Services;
using Order.Persistence.Context;
using Order.Persistence.Services;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddSingleton<IRedisCacheService, RedisCacheService>();
builder.Services.AddSingleton<IConnectionMultiplexer>(options =>
    ConnectionMultiplexer.Connect(builder.Configuration["RedisConnection"] ?? "localhost:6379"));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityURL"];
    options.Audience = "OrderResource";
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
        config.ReceiveEndpoint("order-movie-created", e =>
        {
            e.ConfigureConsumer<MovieCreatedConsumer>(context);
        });
        config.ReceiveEndpoint("order-movie-updated", e =>
        {
            e.ConfigureConsumer<MovieUpdatedConsumer>(context);
        });
        config.ReceiveEndpoint("order-movie-deleted", e =>
        {
            e.ConfigureConsumer<MovieDeletedConsumer>(context);
        });
    });
});

builder.Services.ApplicationService(builder.Configuration);
builder.Services.PresentationService(builder.Configuration);

builder.Services.AddGrpc();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapGrpcService<OrderService>();

app.Run();
