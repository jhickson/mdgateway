using MarketDataGateway.DataTypes;

namespace MarketDataGateway.Abstractions.Services;

/// <summary>
/// Adapter to marshall calls to and responses from the market data validator.
/// </summary>
public interface IMarketDataValidatorAdapter
{
    Task<ValidationStatusCode> Validate(MarketData marketData);
}