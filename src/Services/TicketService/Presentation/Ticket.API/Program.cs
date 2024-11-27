using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Ticket.API.Registrations;
using Ticket.Application.Common.Extensions;
using Ticket.Application.Consumers;
using Ticket.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    .UseSnakeCaseNamingConvention();
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityURL"];
    options.Audience = "TicketResource";
});

builder.Services.ApplicationService(builder.Configuration);
builder.Services.PresentationService(builder.Configuration);

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<TicketUpdatedConsumer>();

    options.AddEntityFrameworkOutbox<ApplicationContext>(x =>
    {
        x.QueryDelay = TimeSpan.FromSeconds(10);
        x.UsePostgres();
        x.UseBusOutbox();
    });

    options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));
    options.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        config.ReceiveEndpoint("ticket-ticket-updated", e =>
        {
            e.ConfigureConsumer<TicketUpdatedConsumer>(context);
        });

        config.ConfigureEndpoints(context);
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
