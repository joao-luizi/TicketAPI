using Castle.Core.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Ticketing.Api.Contracts.Request;
using Ticketing.Api.Controllers;
using Ticketing.Api.DependencyInjection;
using Ticketing.Application.DependencyInjection;
using Ticketing.Application.UseCases.User.CreateUser;

using Ticketing.Infrastructure.DependencyInjection;

namespace Ticketing.Api.Tests.Controllers
{
    public class UserControllerTests
    {
        [Fact]
        public async Task UserControllerShouldReturn201WhenUserIsAuthenticatedAndAdmin()
        {
            var useCaseMock = new Mock<ICreateUserUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateUserOutput { Success = true, UserId = 1, Detail = "ok" });

            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "admin");

            var req = new CreateUserRequest { username = "usernametest", password = "!#$Teste123", email = "teste@test.com", fullName = "teste UserNameTest"};
            var response = await client.PostAsJsonAsync("/users", req);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        [Fact]
        public async Task UserControllerShouldReturns403WhenUserIsAuthenticatedButNotAdmin()
        {
            var useCaseMock = new Mock<ICreateUserUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new CreateUserOutput { Success = true, UserId = 1, Detail = "ok" });

            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "user");

            var req = new CreateUserRequest { username = "u", password = "p" };
            var response = await client.PostAsJsonAsync("/users", req);

            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }

        [Fact]
        public async Task UserControllerShouldReturn401WhenRequestHasNoToken()
        {
            var useCaseMock = new Mock<ICreateUserUseCase>();
            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();

            var req = new CreateUserRequest { username = "u", password = "p" };
            var response = await client.PostAsJsonAsync("/users", req);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task USerControllerShouldReturnProblemWhenUseCaseThrows()
        {
            Mock<ICreateUserUseCase> useCaseMock = new Mock<ICreateUserUseCase>();
            useCaseMock
                .Setup(x => x.Execute(It.IsAny<CreateUserInput>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("erro"));
            var app = BuildApp(useCaseMock.Object);
            var client = app.GetTestClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "admin");
            var request = new CreateUserRequest { username = "usernametest", password = "!#$Teste123", email = "teste@test.com", fullName = "teste UserNameTest" };
            var response = await client.PostAsJsonAsync("/users", request);
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        }
        private static WebApplication BuildApp(ICreateUserUseCase? useCase = null)
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();

            // Add controllers and the specific controller assembly
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(AuthController).Assembly);

            // Register application services used by controllers (same as your existing setup)
            builder.Services.AddApi();
            builder.Services.AddApplication();
            builder.Services.AddInfra(builder.Configuration);

            // Register the use case under test
            builder.Services.AddSingleton(useCase ?? Mock.Of<ICreateUserUseCase>());

            builder.Services.AddLogging();
            // Add test authentication/authorization
            builder.Services.AddAuthentication("Test")
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>("Test", options => { });
            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Ensure authentication/authorization middleware are used
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.StartAsync().GetAwaiter().GetResult();

            return app;
        }

        private class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
                public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                    Microsoft.Extensions.Logging.ILoggerFactory logger, // fully-qualified type
                    UrlEncoder encoder, ISystemClock clock)
                    : base(options, logger, encoder, clock) { }

                protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                if (!Request.Headers.TryGetValue("Authorization", out var headerValues))
                    return Task.FromResult(AuthenticateResult.NoResult());

                var header = headerValues.FirstOrDefault();
                if (string.IsNullOrEmpty(header) || !header.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                    return Task.FromResult(AuthenticateResult.NoResult());

                var token = header.Substring("Bearer ".Length).Trim();

                if (string.Equals(token, "admin", StringComparison.OrdinalIgnoreCase))
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, "admin"),
                        new Claim(ClaimTypes.Role, "Admin")
                    };
                    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                if (string.Equals(token, "user", StringComparison.OrdinalIgnoreCase))
                {
                    var claims = new[] {
                        new Claim(ClaimTypes.Name, "user")
                        // no Admin role
                    };
                    var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, Scheme.Name));
                    var ticket = new AuthenticationTicket(principal, Scheme.Name);
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                // invalid token -> explicit fail
                return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
            }
        }
    }
}

