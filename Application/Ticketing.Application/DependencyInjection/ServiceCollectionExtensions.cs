using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.UseCases.Authentication;
using Ticketing.Application.UseCases.Ticket.CreateTicket;
using Ticketing.Application.UseCases.User.CreateUser;


namespace Ticketing.Application.DependencyInjection
{
    public static  class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ICreateTicketUseCase, CreateTicketUseCase>();
            services.AddScoped<ICreateUserUseCase, CreateUserUseCase>();
            services.AddScoped<ILoginUseCase, LoginUseCase>();
          
            return services;
        }
    }
}
