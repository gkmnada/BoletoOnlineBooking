using Microsoft.EntityFrameworkCore;
using Ticket.API.Registrations;
using Ticket.Application.Common.Extensions;
using Ticket.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.ApplicationService(builder.Configuration);
builder.Services.PresentationService(builder.Configuration);

builder.Services.AddControllers();

//builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    //app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
