using System.Text.Json.Serialization;

namespace MarketDataGateway.DataTypes;

// These two attributes shouldn't be necessary as they're on MarketData but there's an issue in aspnet core that 
// means we need to do this otherwise the MarketDataType is not always written out to JSON (https://github.com/dotnet/runtime/issues/77532)
[JsonPolymorphic(TypeDiscriminatorPropertyName = "marketDataType")]
[JsonDerivedType(typeof(FxQuote), nameof(FxQuote))]
public record FxQuote : MarketData
{
    public required CurrencyPair CurrencyPair { get; init; }
    public required decimal Bid { get; init; }
    public required decimal Ask { get; init; }
}