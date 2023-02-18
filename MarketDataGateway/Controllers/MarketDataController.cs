using System.Net;
using MarketDataGateway.Abstractions.Services;
using MarketDataGateway.DataTypes;
using MarketDataGateway.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace MarketDataGateway.Controllers;

[ApiController]
public class MarketDataController : ControllerBase
{
    private readonly IMarketDataGateway _marketDataGateway;

    public MarketDataController(IMarketDataGateway marketDataGateway)
    {
        _marketDataGateway = marketDataGateway;
    }

    [HttpPost]
    [Route("[controller]")]
    public async Task<ContributionResponseDto> ContributeMarketData([FromBody] MarketData marketData)
    {
        var (reference, statusCode) = await _marketDataGateway.ContributeMarketData(marketData);

        return new ContributionResponseDto(statusCode.ToString(), reference?.RawValue);
    }

    [HttpGet]
    [Route("[controller]/{reference}")]
    public async Task<ActionResult<MarketData>> GetMarketData(string reference)
    {
        var marketData = await _marketDataGateway.GetMarketData(new MarketDataReference(reference));

        if (marketData is null)
        {
            return NotFound(reference);
        }

        return marketData;
    }
}