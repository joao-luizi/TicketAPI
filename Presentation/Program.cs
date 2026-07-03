using Microsoft.EntityFrameworkCore;
using Serilog;
using Ticketing.Api.DependencyInjection;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.DependencyInjection;
using Ticketing.Infrastructure.DependencyInjection;
using Ticketing.Infrastructure.Persistence.Context;



var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .ReadFrom.Configuration(context.Configuration);
});


builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddApi();
builder.Services.AddApplication();
builder.Services.AddInfra(builder.Configuration);
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSerilogRequestLogging();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketingDbContext>();
    await db.Database.MigrateAsync();

    var seeder = scope.ServiceProvider.GetRequiredService<IDbSeeder>();

    await seeder.SeedAsync();

}

// Configure the HTTP request pipeline.
// Swagger enabled in all environments because this is an API-only service
//if (app.Environment.IsDevelopment())
//{
//app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();
//}

app.MapControllers();


app.Run();


