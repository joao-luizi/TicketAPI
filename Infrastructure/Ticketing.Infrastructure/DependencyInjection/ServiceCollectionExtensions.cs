using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Abstractions.Persistence;
using Ticketing.Infrastructure.Persistence.Repositories;

namespace Ticketing.Infrastructure.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfra(this IServiceCollection services)
        {
            services.AddScoped<ITicketRepository, TicketRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
    }
}
