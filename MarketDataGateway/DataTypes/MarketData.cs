using System.Text.Json.Serialization;

namespace MarketDataGateway.DataTypes;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "marketDataType")]
[JsonDerivedType(typeof(FxQuote), nameof(FxQuote))]
public record MarketData
{
    public required DateTimeOffset Timestamp { get; init; }
}