using System.Text.Json;
using System.Text.Json.Serialization;
using MarketDataGateway.DataTypes;

namespace MarketDataGatewayTests.DataTypes;

[TestFixture]
public class CurrencyPairTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().WithDataTypes();
    }

    private IFixture _fixture = null!;

    [Test]
    [TestCase("GBP/USD", "GBP", "USD")]
    [TestCase("USDEUR", "USD", "EUR")]
    public void Given_ValidCurrencyPairString_When_Parse_Then_ExtractCorrectPairs(
        string pairText, string baseText, string quoteText
    )
    {
        var subject = CurrencyPair.Parse(pairText);

        var baseCurrencyCode = new CurrencyCode(baseText);
        var quoteCurrencyCode = new CurrencyCode(quoteText);
        
        subject.Base.Should().Be(baseCurrencyCode);
        subject.Quote.Should().Be(quoteCurrencyCode);
    }

    [Test]
    public void Given_Json_When_ContainsCurrencyPair_Then_Deserialize()
    {
        var dto = _fixture.Create<Dto>();

        var json = $@"{{""Pair"":""{dto.Pair.Base}/{dto.Pair.Quote}""}}";

        var actual = JsonSerializer.Deserialize<Dto>(json);

        actual.Should().NotBeNull();
        actual!.Pair.Should().Be(dto.Pair);
    }
    
    [Test]
    public void Given_Pair_When_SerializeToJson_Then_WriteCorrectly()
    {
        var dto = _fixture.Create<Dto>();

        var expectedJson = $@"{{""Pair"":""{dto.Pair.Base}/{dto.Pair.Quote}""}}";

        var actualJson = JsonSerializer.Serialize(dto);

        actualJson.Should().Be(expectedJson);
    }

    private sealed record Dto(CurrencyPair Pair);
}