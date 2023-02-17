using System.Text.Json;
using MarketDataGateway.DataTypes;

namespace MarketDataGatewayTests.DataTypes;

[TestFixture]
public class FxQuoteTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().WithDataTypes();
    }

    private IFixture _fixture = null!;

    [Test]
    public void Given_ValidFxQuoteJson_When_DeserializeAsMarketData_Then_CorrectlyInflateToFxQuote()
    {
        var timestamp = _fixture.Create<DateTimeOffset>();
        var pair = _fixture.Create<CurrencyPair>();
        var bid = _fixture.Create<decimal>();
        var ask = _fixture.Create<decimal>();
        var json = $@"
{{
    ""marketDataType"": ""FxQuote"",
    ""currencyPair"": ""{pair}"",
    ""bid"": {bid},
    ""ask"": {ask},
    ""timestamp"": ""{timestamp:O}""
}}
";

        var actual =
            JsonSerializer.Deserialize<MarketData>(
                json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

        actual.Should().NotBeNull();
        actual!.Timestamp.Should().Be(timestamp);
        actual.Should().BeOfType<FxQuote>();
        actual.As<FxQuote>()
              .Should()
              .BeEquivalentTo(
                  new
                  {
                      Timestamp = timestamp,
                      CurrencyPair = pair,
                      Bid = bid,
                      Ask = ask
                  }
              );
    }
    
}