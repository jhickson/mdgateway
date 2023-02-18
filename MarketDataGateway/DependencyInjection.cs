using MarketDataGateway.Abstractions.Services;
using MarketDataGateway.Services;

namespace MarketDataGateway;

public static class DependencyInjection
{
    public static IServiceCollection AddMarketDataGatewayServices(this IServiceCollection services)
    {
        return services.AddTransient<IMarketDataGateway, Services.MarketDataGateway>()
                       .AddTransient<IMarketDataRepository, InMemoryMarketDataRepository>()
                       .AddTransient<IMarketDataValidatorAdapter, MarketDataValidatorSimulator>();
    }
}