using MarketDataGateway.DataTypes;
using static MoreLinq.MoreEnumerable;

namespace MarketDataGatewayTests.DataTypes;

[TestFixture]
public class MarketDataReferenceTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    private IFixture _fixture = null!;

    [Test]
    public void Given_String_When_UseToCreate_Then_ShouldBeAbleToRetrieveString()
    {
        var text = _fixture.Create<string>();

        var subject = new MarketDataReference(text);

        subject.RawValue.Should().Be(text);
        subject.ToString().Should().Be(text);
    }

    [Test]
    public void Given_InvalidText_When_UseToCreateReference_Then_Throw([Values("", " ")] string text)
    {
        FluentActions.Invoking(() => new MarketDataReference(text))
                     .Should()
                     .Throw<Exception>()
                     .WithMessage("Invalid market data reference.");
    }

    [Test]
    public void Given_MultipleNewReferences_When_Compare_Then_ShouldBeDistinct()
    {
        var subjects =
            Generate(MarketDataReference.NewReference(), _ => MarketDataReference.NewReference())
                .Take(1000)
                .ToArray();

        subjects.Should().OnlyHaveUniqueItems();
    }
}