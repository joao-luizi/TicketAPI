using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Infrastructure.Persistence.Context;
using Ticketing.Infrastructure.Persistence.Repositories;

namespace Ticketing.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfra(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("TicketingDB")
            ?? throw new InvalidOperationException("Connection string 'TicketingDB' was not found.");

            services.AddDbContext<TicketingDbContext>(options =>
                options.UseNpgsql(connectionString));
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
