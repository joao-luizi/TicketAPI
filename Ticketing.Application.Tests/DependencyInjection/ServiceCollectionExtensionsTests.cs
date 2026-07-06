using Microsoft.Extensions.DependencyInjection;
using Ticketing.Application.DependencyInjection;
using Ticketing.Application.UseCases.Authentication;
using Ticketing.Application.UseCases.Ticket.CreateTicket;
using Ticketing.Application.UseCases.User.CreateUser;

namespace Ticketing.Application.Tests.DependencyInjection
{
    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddApplicationServices_ShouldRegisterServices()
        {
            // Arrange
            var services = new ServiceCollection();
            // Act
            services.AddApplication();
           
            Assert.Contains(services, x =>
                       x.ServiceType == typeof(ICreateUserUseCase)
                       && x.ImplementationType == typeof(CreateUserUseCase));
            Assert.Contains(services, x =>
                      x.ServiceType == typeof(ICreateTicketUseCase)
                      && x.ImplementationType == typeof(CreateTicketUseCase));
            Assert.Contains(services, x =>
              x.ServiceType == typeof(ILoginUseCase)
              && x.ImplementationType == typeof(LoginUseCase));
        }
    }
}
