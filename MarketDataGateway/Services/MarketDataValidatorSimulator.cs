using MarketDataGateway.Abstractions.Services;
using MarketDataGateway.DataTypes;

namespace MarketDataGateway.Services;

/// <summary>
/// A very basic simulation of a market data validator.
/// </summary>
public sealed class MarketDataValidatorSimulator : IMarketDataValidatorAdapter
{
    public async Task<ValidationStatusCode> Validate(MarketData marketData)
    {
        return
            marketData switch
            {
                FxQuote fxQuote =>
                    fxQuote switch
                    {
                        { Bid: <= 0 } => ValidationStatusCode.NegativeOrZeroFxQuote,
                        { Ask: <= 0 } => ValidationStatusCode.NegativeOrZeroFxQuote,
                        _ => ValidationStatusCode.Ok
                    },
                _ => ValidationStatusCode.UnsupportedMarketDataType
            };
    }
}