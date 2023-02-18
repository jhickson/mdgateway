using MarketDataGateway.DataTypes;

namespace MarketDataGateway.Abstractions.Services;

public interface IMarketDataGateway
{
    Task<(MarketDataReference?, ValidationStatusCode)> ContributeMarketData(MarketData marketData);
    Task<MarketData?> GetMarketData(MarketDataReference reference);
}