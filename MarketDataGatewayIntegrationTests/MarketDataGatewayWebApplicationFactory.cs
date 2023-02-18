using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace MarketDataGatewayIntegrationTests;

internal sealed class MarketDataGatewayWebApplicationFactory : WebApplicationFactory<Program>
{
    private readonly List<(Type ServiceType, object Fake)> _fakeServices = new List<(Type ServiceType, object Fake)>();

    public void AddFakedService<TService>(TService service) where TService : class
    {
        _fakeServices.Add((typeof(TService), service));
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(
            services =>
            {
                foreach (var (serviceType, fake) in _fakeServices)
                {
                    services.RemoveAll(serviceType);
                    services.AddSingleton(serviceType, fake);
                }
            }
        );

        builder.UseEnvironment("Development");
    }
}