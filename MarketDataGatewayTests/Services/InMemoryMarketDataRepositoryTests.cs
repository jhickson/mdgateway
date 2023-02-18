using MarketDataGateway.DataTypes;
using MarketDataGateway.Services;

namespace MarketDataGatewayTests.Services;

[TestFixture]
public class InMemoryMarketDataRepositoryTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().WithDataTypes().WithFakeItEasy();
    }

    private IFixture _fixture = null!;

    [Test]
    public async Task Given_UnrecognisedMarketDataReference_When_UseToGetData_Then_ReturnNull()
    {
        var subject = _fixture.Create<InMemoryMarketDataRepository>();

        var data = await subject.GetMarketData(_fixture.Create<MarketDataReference>());

        data.Should().BeNull();
    }

    [Test]
    public async Task Given_MarketData_When_StoreIt_Then_ShouldBeAbleToRetrieveIt()
    {
        var expectedData = _fixture.Create<MarketData>();
        var marketDataReference = _fixture.Create<MarketDataReference>();

        var subject = _fixture.Create<InMemoryMarketDataRepository>();

        await subject.StoreMarketData(marketDataReference, expectedData);

        var data = await subject.GetMarketData(marketDataReference);

        data.Should().Be(expectedData);
    }
}