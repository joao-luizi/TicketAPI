using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ticketing.Infrastructure.DependencyInjection;

namespace Ticketing.Infrastructure.Tests
{
    public  class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddInfrastructure_ShouldThrow_WhenConnectionStringIsMissing()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string?>()).Build();

            Assert.Throws<InvalidOperationException>(() => services.AddInfra(configuration));
        }
    }
}
