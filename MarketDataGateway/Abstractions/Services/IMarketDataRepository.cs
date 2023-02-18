using MarketDataGateway.DataTypes;

namespace MarketDataGateway.Abstractions.Services;

/// <summary>
///     Interface for market data storage implementations.
/// </summary>
public interface IMarketDataRepository
{
    Task StoreMarketData(MarketDataReference reference, MarketData marketData);

    Task<MarketData?> GetMarketData(MarketDataReference reference);
}