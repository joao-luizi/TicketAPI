using Microsoft.EntityFrameworkCore;
using Ticketing.Api.DependencyInjection;
using Ticketing.Application.DependencyInjection;
using Ticketing.Infrastructure.DependencyInjection;
using Ticketing.Infrastructure.Persistence.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
//builder.Services.AddOpenApi();
builder.Services.AddApi();
builder.Services.AddApplication();
builder.Services.AddInfra(builder.Configuration);
builder.Services.AddSwaggerGen();
var app = builder.Build();


using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TicketingDbContext>();
    db.Database.Migrate();
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


