namespace MarketDataGateway.DataTypes;

public sealed record FxQuote : MarketData
{
    public required CurrencyPair CurrencyPair { get; init; }
    public required decimal Bid { get; init; }
    public required decimal Ask { get; init; }
}