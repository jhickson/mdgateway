namespace MarketDataGateway.DataTypes;

/// <summary>
///     Represents a currency code.
/// </summary>
/// <remarks>
///     The code should be three letters long (A..Z) though lowercase letters will be converted.
/// </remarks>
/// <param name="RawValue">The raw string version of the 3-character code.</param>
public readonly record struct CurrencyCode(string RawValue)
{
    private const int ExpectedLength = 3;

    public string RawValue { get; } = Validate(RawValue.ToUpper());

    private static string Validate(string text)
    {
        if (text.Length == ExpectedLength && text.All(c => char.IsBetween(c, 'A', 'Z')))
        {
            return text;
        }

        throw new Exception("Invalid currency code.");
    }

    public override string ToString()
    {
        return RawValue;
    }
}