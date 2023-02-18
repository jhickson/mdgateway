using MarketDataGateway.Abstractions.Services;
using MarketDataGateway.DataTypes;

namespace MarketDataGateway.Services;

public sealed class MarketDataGateway
{
    private readonly IMarketDataValidatorAdapter _validator;
    private readonly IMarketDataRepository _marketDataRepository;

    public MarketDataGateway(IMarketDataValidatorAdapter validator, IMarketDataRepository marketDataRepository)
    {
        _validator = validator;
        _marketDataRepository = marketDataRepository;
    }

    public async Task<(MarketDataReference?, ValidationStatusCode)> ContributeMarketData(MarketData marketData)
    {
        var statusCode = await _validator.Validate(marketData);

        if (statusCode != ValidationStatusCode.Ok)
        {
            return (null, statusCode);
        }

        var reference = MarketDataReference.NewReference();

        await _marketDataRepository.StoreMarketData(reference, marketData);

        return (reference, statusCode);
    }

    public Task<MarketData?> GetMarketData(MarketDataReference reference)
    {
        return _marketDataRepository.GetMarketData(reference);
    }
}