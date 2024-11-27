using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Payment.API.Clients;
using Payment.API.Consumers;
using Payment.API.Repositories.Payment;
using Payment.API.Services;
using Payment.API.Settings;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddMediatR(options =>
{
    options.RegisterServicesFromAssemblies(Assembly.GetExecutingAssembly());
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = builder.Configuration["IdentityURL"];
    options.Audience = "PaymentResource";
});

builder.Services.AddMassTransit(options =>
{
    options.AddConsumer<BookingCheckoutConsumer>();

    options.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(false));
    options.UsingRabbitMq((context, config) =>
    {
        config.Host(builder.Configuration["RabbitMQ"], "/", host =>
        {
            host.Username("guest");
            host.Password("guest");
        });

        config.ReceiveEndpoint("payment-booking-checkout", endpoint =>
        {
            endpoint.ConfigureConsumer<BookingCheckoutConsumer>(context);
        });
    });
});

builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<OrderClient>();

builder.Services.Configure<DatabaseSettings>(builder.Configuration.GetSection("DatabaseSettings"));

builder.Services.AddScoped<IDatabaseSettings>(options =>
{
    return options.GetRequiredService<IOptions<DatabaseSettings>>().Value;
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
