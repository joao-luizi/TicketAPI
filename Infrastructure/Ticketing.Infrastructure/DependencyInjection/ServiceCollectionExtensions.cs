using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Application.Abstractions.Security;
using Ticketing.Infrastructure.Persistence.Context;
using Ticketing.Infrastructure.Persistence.Repositories;
using Ticketing.Infrastructure.Services.Security;

namespace Ticketing.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("TicketingDB")
            ?? throw new InvalidOperationException("Connection string 'TicketingDB' was not found.");

            services.AddDbContext<TicketingDbContext>(options =>
                options.UseNpgsql(connectionString, 
                npgsqlOptions => 
                npgsqlOptions.EnableRetryOnFailure(maxRetryCount: 10, 
                maxRetryDelay: TimeSpan.FromSeconds(30), 
                errorCodesToAdd: null)));

            services.Configure<JwtOptions>(
                configuration.GetSection("Jwt"));

            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
            services.AddScoped<IDbSeeder, DbSeeder>();
            services.AddScoped<ITokenService, JwtTokenService>();
            return services;
        }
    }
}
