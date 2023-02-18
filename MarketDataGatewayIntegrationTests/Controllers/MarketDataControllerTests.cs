using System.Net;
using System.Text.Json;
using AutoFixture;
using FakeItEasy;
using FluentAssertions;
using MarketDataGateway.Abstractions.Services;
using MarketDataGateway.Controllers;
using MarketDataGateway.DataTypes;
using MarketDataGatewayTests;
using Microsoft.AspNetCore.Mvc.Testing;

namespace MarketDataGatewayIntegrationTests.Controllers;

[TestFixture]
public class MarketDataControllerTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture().WithDataTypes().WithFakeItEasy();
    }

    private IFixture _fixture = null!;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions
                                                                          {
                                                                              PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                                                                          };

    [Test]
    public void When_StartServer_Then_ShouldBeAbleToResolveController()
    {
        var webApplicationFactory =
            new WebApplicationFactory<Program>().WithWebHostBuilder(
                // Without calling AddControllersAsServices, the controllers are only loaded when
                // their route is hit so checking their dependencies are resolved would require the
                // actual use of an endpoint with the full (unfaked) services in use, which is not
                // always desirable.
                x => x.ConfigureServices(s => s.AddControllers().AddControllersAsServices())
            );

        webApplicationFactory.Server.Invoking(x => x.Services.GetRequiredService<MarketDataController>())
                             .Should()
                             .NotThrow();
    }

    [Test]
    public async Task
        Given_MarketDataJson_When_PostToMarketDataEndpoint_Then_InstigateNewContributionProcessingAndReturnResult(
            [Values] bool validationSucceeds
        )
    {
        var webApplicationFactory = new MarketDataGatewayWebApplicationFactory();

        var marketDataGateway = _fixture.Create<IMarketDataGateway>();

        webApplicationFactory.AddFakedService(marketDataGateway);

        var httpClient = webApplicationFactory.CreateClient();

        var fxQuote = _fixture.Create<FxQuote>();
        var marketDataReference =
            validationSucceeds ? _fixture.Create<MarketDataReference>() : default(MarketDataReference?);
        var validationStatusCode = _fixture.Create<ValidationStatusCode>();

        A.CallTo(() => marketDataGateway.ContributeMarketData(fxQuote))
         .Returns((marketDataReference, validationStatusCode));

        var jsonContent = JsonContent.Create((MarketData)fxQuote, options: JsonSerializerOptions);
        var response = await httpClient.PostAsync("/marketdata", jsonContent);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = (await response.Content.ReadFromJsonAsync<NewContributionResponseDto>())!;

        responseDto.StatusCode.Should().Be(validationStatusCode.ToString());
        responseDto.Reference.Should().Be(marketDataReference?.RawValue);
    }

    [Test]
    public async Task Given_MarketDataReference_When_MarketDataExists_Then_ShouldBeAbleToRetrieveIt()
    {
        var webApplicationFactory = new MarketDataGatewayWebApplicationFactory();

        var marketDataGateway = _fixture.Create<IMarketDataGateway>();

        webApplicationFactory.AddFakedService(marketDataGateway);

        var httpClient = webApplicationFactory.CreateClient();

        var marketDataReference = _fixture.Create<MarketDataReference>();
        var marketData = (MarketData)_fixture.Create<FxQuote>();

        A.CallTo(() => marketDataGateway.GetMarketData(marketDataReference))
         .Returns(marketData);

        var response = await httpClient.GetAsync($"/marketdata/{marketDataReference}");

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseDto = await response.Content.ReadAsStringAsync();

        responseDto.Should().Be(JsonSerializer.Serialize(marketData, JsonSerializerOptions));
    }

    [Test]
    public async Task Given_MarketDataReference_When_MarketDataDoesNotExist_Then_ReturnNotFound()
    {
        var webApplicationFactory = new MarketDataGatewayWebApplicationFactory();

        var marketDataGateway = _fixture.Create<IMarketDataGateway>();

        webApplicationFactory.AddFakedService(marketDataGateway);

        var httpClient = webApplicationFactory.CreateClient();

        var marketDataReference = _fixture.Create<MarketDataReference>();

        A.CallTo(() => marketDataGateway.GetMarketData(marketDataReference))
         .Returns(default(MarketData?));

        var response = await httpClient.GetAsync($"/marketdata/{marketDataReference}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    private sealed record NewContributionResponseDto
    {
        public required string StatusCode { get; init; }

        public string? Reference { get; init; }
    }
}