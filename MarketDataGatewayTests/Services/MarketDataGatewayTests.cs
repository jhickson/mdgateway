using FakeItEasy;
using MarketDataGateway.Abstractions.Services;
using MarketDataGateway.DataTypes;

namespace MarketDataGatewayTests.Services;

[TestFixture]
public class MarketDataGatewayTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().WithDataTypes().WithFakeItEasy();
    }

    private IFixture _fixture = null!;

    [Test]
    public async Task Given_NewMarketData_When_ValidatorDeemsItValid_Then_StoreDataAndReturnReference()
    {
        var validatorAdapter = _fixture.Freeze<IMarketDataValidatorAdapter>();
        var marketDataRepository = _fixture.Freeze<IMarketDataRepository>();
        var fxQuote = _fixture.Create<FxQuote>();

        A.CallTo(() => validatorAdapter.Validate(fxQuote)).Returns(ValidationStatusCode.Ok);

        var subject = _fixture.Create<MarketDataGateway.Services.MarketDataGateway>();

        var (reference, statusCode) = await subject.ContributeMarketData(fxQuote);

        reference.Should().NotBeNull();
        reference!.Should().NotBe(default(MarketDataReference));
        statusCode.Should().Be(ValidationStatusCode.Ok);

        A.CallTo(() => validatorAdapter.Validate(fxQuote))
         .MustHaveHappenedOnceExactly()
         .Then(
             A.CallTo(() => marketDataRepository.StoreMarketData(reference!.Value, fxQuote))
              .MustHaveHappenedOnceExactly()
         );
    }

    [Test]
    public async Task Given_NewMarketData_When_ValidateDeemsItInvalid_Then_StoreNothingAndReturnStatus()
    {
        var validatorAdapter = _fixture.Freeze<IMarketDataValidatorAdapter>();
        var marketDataRepository = _fixture.Freeze<IMarketDataRepository>();
        var fxQuote = _fixture.Create<FxQuote>();
        var validationStatusCode = _fixture.CreateMany<ValidationStatusCode>().First(c => c != ValidationStatusCode.Ok);

        A.CallTo(() => validatorAdapter.Validate(fxQuote)).Returns(validationStatusCode);

        var subject = _fixture.Create<MarketDataGateway.Services.MarketDataGateway>();

        var (reference, statusCode) = await subject.ContributeMarketData(fxQuote);

        reference.Should().BeNull();
        statusCode.Should().Be(validationStatusCode);

        A.CallTo(() => validatorAdapter.Validate(fxQuote))
         .MustHaveHappenedOnceExactly();

        A.CallTo(() => marketDataRepository.StoreMarketData(A<MarketDataReference>._, fxQuote))
         .MustNotHaveHappened();
    }

    [Test]
    public async Task Given_MarketDataReference_When_UseItToRetrieveData_Then_ReturnWhateverRepositoryReturns(
        [Values] bool dataExists
    )
    {
        var marketDataRepository = _fixture.Freeze<IMarketDataRepository>();
        var expectedResponse = dataExists ? _fixture.Create<FxQuote>() : default(MarketData?);

        var marketDataReference = _fixture.Create<MarketDataReference>();

        A.CallTo(() => marketDataRepository.GetMarketData(marketDataReference))
         .Returns(expectedResponse);

        var subject = _fixture.Create<MarketDataGateway.Services.MarketDataGateway>();

        var response = await subject.GetMarketData(marketDataReference);

        response.Should().Be(expectedResponse);
    }
}