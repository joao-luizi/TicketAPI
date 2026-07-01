using Ticketing.Api.DependencyInjection;
using Ticketing.Application.DependencyInjection;
using Ticketing.Infrastructure.DependencyInjection;

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


