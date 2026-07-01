using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.Abstractions.Security;
using Ticketing.Application.UseCases.CreateTicket;
using Ticketing.Application.UseCases.CreateUser;


namespace Ticketing.Application.DependencyInjection
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            
            return services;
        }
    }
}
