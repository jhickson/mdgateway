using System.Text.Json;
using System.Text.Json.Serialization;

namespace MarketDataGateway.DataTypes;

[JsonConverter(typeof(Converter))]
public sealed record CurrencyPair(CurrencyCode Base, CurrencyCode Quote)
{
    public static CurrencyPair Parse(string text)
    {
        return text.Split('/') switch
        {
            [var b, var q] => new CurrencyPair(new CurrencyCode(b), new CurrencyCode(q)),
            [{ Length: 6 } x] => new CurrencyPair(new CurrencyCode(x[..3]), new CurrencyCode(x[3..])),
            _ => throw new Exception("Invalid currency pair.")
        };
    }

    public override string ToString()
    {
        return $"{Base}/{Quote}";
    }

    private sealed class Converter : JsonConverter<CurrencyPair>
    {
        public override CurrencyPair? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return Parse(reader.GetString()!);
        }

        public override void Write(Utf8JsonWriter writer, CurrencyPair value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}