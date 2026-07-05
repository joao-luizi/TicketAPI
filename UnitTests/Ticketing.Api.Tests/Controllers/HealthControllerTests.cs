using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Ticketing.Api.Controllers;
using Ticketing.Api.DependencyInjection;
using Ticketing.Application.DependencyInjection;
using Ticketing.Infrastructure.DependencyInjection;

namespace Ticketing.Api.Tests.Controllers
{
    public class HealthControllerTests
    {
        [Fact]
        public async Task HealthController_ShouldReturnOkay()
        {
            await using var app = BuildApp();

            var response = await app.GetTestClient().GetAsync("/health");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        private static WebApplication BuildApp()
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();
            builder.Services.AddControllers()
                .AddApplicationPart(typeof(HealthController).Assembly);
            builder.Services.AddApi();
            builder.Services.AddApplication();
            builder.Services.AddInfra(builder.Configuration);

            var app = builder.Build();
            app.MapControllers();
            app.StartAsync().GetAwaiter().GetResult();
            return app;
        }
    }
}
