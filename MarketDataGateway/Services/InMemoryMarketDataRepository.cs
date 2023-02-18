using System.Collections.Concurrent;
using MarketDataGateway.Abstractions.Services;
using MarketDataGateway.DataTypes;

namespace MarketDataGateway.Services;

/// <summary>
/// Very simplistic market data store.
/// </summary>
public sealed class InMemoryMarketDataRepository : IMarketDataRepository
{
    private readonly ConcurrentDictionary<MarketDataReference, MarketData> _store =
        new ConcurrentDictionary<MarketDataReference, MarketData>();
    
    public async Task StoreMarketData(MarketDataReference reference, MarketData marketData)
    {
        _store.TryAdd(reference, marketData);
    }

    public async Task<MarketData?> GetMarketData(MarketDataReference reference)
    {
        return _store.TryGetValue(reference, out var value) ? value : null;
    }
}