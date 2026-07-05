using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Controllers;
using Ticketing.Api.DependencyInjection;
using Ticketing.Application.DependencyInjection;
using Ticketing.Application.UseCases.Ticket.CreateTicket;
using Ticketing.Infrastructure.DependencyInjection;
using Ticketing.Domain.Enums;

namespace Ticketing.Api.Tests.Controllers
{
    public class TicketControllerTests
    {
        [Fact]
        public async Task TicketController_ShouldReturnBadRequestWhenUserNotFound()
        {
            var useCaseMock = new Mock<ICreateTicketUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<CreateTicketInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateTicketOutput()
                {
                    TicketId = 0,
                    Detail = "User not found",
                    FailureType = CreateTicketFailureType.UserNotFound
                });
            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();
            var request = new CreateTicketRequest
            {
                Title = "Test Title",
                Description = "Test Description",
                UserEmail = "admin@admin.com"
            };
        }
        [Fact]
        public async Task TicketController_ShouldReturn409ConflirWhenUseCaseHasDuplicateTicket()
        {
            var useCaseMock = new Mock<ICreateTicketUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<CreateTicketInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateTicketOutput()
                {
                    TicketId = 0,
                    Detail = "Duplicate ticket",
                    FailureType = CreateTicketFailureType.DuplicateTicket
                });
            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();
            var request = new CreateTicketRequest
            {
                Title = "Test Title",
                Description = "Test Description",
                UserEmail = "admin@admin.com"
            };
            var response = await client.PostAsJsonAsync("/tickets", request);

            Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
        }

        [Fact]
        public async Task TicketController_ShouldReturnProblemWhenUseCaseThrows()
        {
            var useCaseMock = new Mock<ICreateTicketUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<CreateTicketInput>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("erro"));
            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();
            var request = new CreateTicketRequest
            {
                Title = "Test Title",
                Description = "Test Description",
                UserEmail = "Teste@teste.com"
            };
            var response = await client.PostAsJsonAsync("/tickets", request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task TicketController_ShouldReturn201WhenUseCaseSucceeds() 
        { 
        Mock<ICreateTicketUseCase> useCaseMock = new Mock<ICreateTicketUseCase>();
            useCaseMock.Setup(x => x.Execute(It.IsAny<CreateTicketInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateTicketOutput { Success = true, TicketId = 1, Detail = "Ticket created successfully" });
            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();
            var request = new CreateTicketRequest
            {
                Title = "Test Title",
                Description = "Test Description",
                UserEmail = "Teste@teste.com"
            };
            // Act
            var response = await client.PostAsJsonAsync("/tickets", request);
            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        }

        private static WebApplication BuildApp(ICreateTicketUseCase? useCase = null)
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(AuthController).Assembly);
            builder.Services.AddApi();
            builder.Services.AddApplication();
            builder.Services.AddInfra(builder.Configuration);
            builder.Services.AddSingleton(useCase ?? Mock.Of<ICreateTicketUseCase>());

            var app = builder.Build();
            app.MapControllers();
            app.StartAsync().GetAwaiter().GetResult();
            return app;
        }
    }
}
