using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http.Json;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Controllers;
using Ticketing.Api.DependencyInjection;
using Ticketing.Application.DependencyInjection;
using Ticketing.Application.UseCases.Authentication;
using Ticketing.Application.UseCases.Ticket.CreateTicket;
using Ticketing.Infrastructure.DependencyInjection;

namespace Ticketing.Api.Tests.Controllers
{
    public class AuthControllerTests
    {

        [Fact]
        public async Task AuthController_ShouldReturnOk_WhenUseCaseSucceeds()
        {
            // Arrange
            var mockUseCase = new Mock<ILoginUseCase>();
            mockUseCase.Setup(x => x.Execute(It.IsAny<LoginInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LoginOutput { Success = true, Token = "mock-token", Detail = "Login successful" });
            var app = BuildApp(mockUseCase.Object);
            var client = app.GetTestClient();
            var request = new LoginUserRequest
            {
                UserName = "testuser",
                Password = "password"
            };
            // Act
            var response = await client.PostAsJsonAsync("/auth/login", request);
            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task AuthController_ShouldReturnProblem_WhenUseCaseThrows()
        {
            var useCaseMock = new Mock<ILoginUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<LoginInput>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("erro"));

            await using var app = BuildApp(useCaseMock.Object);

            var response = await app.GetTestClient().PostAsJsonAsync("/auth/login", new LoginRequest
            {
                Email = "email",
                Password = "worng"
            });

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async Task AuthController_ShouldReturnUnauthorized_WhenUseCaseReturnsFailure()
        {
            var useCaseMock = new Mock<ILoginUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<LoginInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new LoginOutput
                {
                    Success = false,
                    Token = null,
                    Detail = "Invalid credentials"
                });
            await using var app = BuildApp(useCaseMock.Object);
            var response = await app.GetTestClient().PostAsJsonAsync("/auth/login", new LoginRequest
            {
                Email = "email",
                Password = "wrong"
            });
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        private static WebApplication BuildApp(ILoginUseCase? useCase = null)
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(AuthController).Assembly);
            builder.Services.AddApi();
            builder.Services.AddApplication();
            builder.Services.AddInfra(builder.Configuration);
            builder.Services.AddSingleton(useCase ?? Mock.Of<ILoginUseCase>());

            var app = builder.Build();
            app.MapControllers();
            app.StartAsync().GetAwaiter().GetResult();
            return app;
        }
    }
}
