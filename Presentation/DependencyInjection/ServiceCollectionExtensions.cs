using FluentValidation;
using Ticketing.Api.Validators;
namespace Ticketing.Api.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateTicketRequestValidator>();
            services.AddValidatorsFromAssemblyContaining<Program>();
            return services;
        }
    }
}
