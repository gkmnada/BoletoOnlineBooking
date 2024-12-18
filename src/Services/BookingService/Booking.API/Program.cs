using Booking.API.Clients;
using Booking.API.Consumers;
using Booking.API.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityURL"];
    options.Audience = "BookingResource";
});

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<TicketCreatedConsumer>();
    options.AddConsumer<PaymentCompletedConsumer>();

    options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));
    options.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        config.ReceiveEndpoint("booking-ticket-created", endpoint =>
        {
            endpoint.ConfigureConsumer<TicketCreatedConsumer>(context);
        });

        config.ReceiveEndpoint("booking-payment-completed", endpoint =>
        {
            endpoint.ConfigureConsumer<PaymentCompletedConsumer>(context);
        });

        config.ConfigureEndpoints(context);
    });
});

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<DiscountClient>();

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
