namespace MarketDataGateway.DataTypes;

/// <summary>
///     A globally-unique market data reference.
/// </summary>
/// <param name="RawValue"></param>
public readonly record struct MarketDataReference(string RawValue)
{
    public string RawValue { get; } = Validate(RawValue);

    private static string Validate(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new Exception("Invalid market data reference.");
        }

        return text;
    }

    public override string ToString()
    {
        return RawValue;
    }

    public static MarketDataReference NewReference()
    {
        return new MarketDataReference(Guid.NewGuid().ToString("N"));
    }
}