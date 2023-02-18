using MarketDataGateway.DataTypes;
using MarketDataGateway.Services;

namespace MarketDataGatewayTests.Services;

[TestFixture]
public class MarketDataValidatorSimulatorTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().WithDataTypes().WithFakeItEasy();
    }

    private IFixture _fixture = null!;

    [Test]
    [TestCase(-1.67, 2.0, ValidationStatusCode.NegativeOrZeroFxQuote)]
    [TestCase(1.67, -2.0, ValidationStatusCode.NegativeOrZeroFxQuote)]
    [TestCase(0, 2.0, ValidationStatusCode.NegativeOrZeroFxQuote)]
    [TestCase(1.0, 0, ValidationStatusCode.NegativeOrZeroFxQuote)]
    [TestCase(1.67, 2.0, ValidationStatusCode.Ok)]
    public async Task Given_FxQuote_When_BidOrAskIsNegative_Then_ReturnCorrectStatus(
        decimal bid, decimal ask, ValidationStatusCode expectedStatusCode
    )
    {
        var fxQuote = _fixture.Create<FxQuote>() with
                      {
                          Ask = ask,
                          Bid = bid
                      };

        var subject = _fixture.Create<MarketDataValidatorSimulator>();

        var statusCode = await subject.Validate(fxQuote);

        statusCode.Should().Be(expectedStatusCode);
    }

    [Test]
    public async Task Given_MarketData_When_UnknownToValidator_Then_ReturnAppropriateStatus()
    {
        var marketData = _fixture.Create<MarketData>();

        var subject = _fixture.Create<MarketDataValidatorSimulator>();

        var statusCode = await subject.Validate(marketData);

        statusCode.Should().Be(ValidationStatusCode.UnsupportedMarketDataType);
    }
}