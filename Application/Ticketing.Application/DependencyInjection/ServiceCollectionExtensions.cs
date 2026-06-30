using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.UseCases.CreateTicket;


namespace Ticketing.Application.DependencyInjection
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
            return services;
        }
    }
}
