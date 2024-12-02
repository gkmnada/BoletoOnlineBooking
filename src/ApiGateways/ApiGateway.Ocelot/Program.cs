using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddAuthentication().AddJwtBearer("OcelotAuthenticationScheme", options =>
{
    options.Authority = builder.Configuration["IdentityURL"];
    options.Audience = "GatewayResource";
});

IConfiguration configuration = new ConfigurationBuilder()
    .AddJsonFile("ocelot.json").Build();

builder.Services.AddOcelot(configuration);

var app = builder.Build();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();
