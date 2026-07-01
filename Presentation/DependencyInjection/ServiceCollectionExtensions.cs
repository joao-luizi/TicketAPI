using FluentValidation;
using FluentValidation.AspNetCore;
using Ticketing.Api.Validators;
namespace Ticketing.Api.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApi(this IServiceCollection services)
        {
            services.AddValidatorsFromAssemblyContaining<CreateTicketRequestValidator>();
            services.AddFluentValidationAutoValidation();
            return services;
        }
    }
}
