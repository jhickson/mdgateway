using MarketDataGateway.DataTypes;

namespace MarketDataGatewayTests.DataTypes;

[TestFixture]
public class CurrencyCodeTests
{
    [SetUp]
    public void SetUp()
    {
        _fixture = new Fixture();
    }

    private IFixture _fixture = null!;

    [Test]
    public void Given_3CharacterString_When_CharactersAreAsciiLetters_Then_ShouldBeAbleToCreateCurrencyCode()
    {
        var text = string.Join(
            "",
            _fixture.CreateMany<char>(1000)
                    .Where(c => char.IsBetween(c, 'A', 'Z') || char.IsBetween(c, 'a', 'z'))
                    .Take(3)
        );

        var subject = new CurrencyCode(text);

        subject.RawValue.Should().Be(text.ToUpper());
        subject.ToString().Should().Be(text.ToUpper());
    }

    [Test]
    public void Given_String_When_Not3CharactersLong_Then_ThrowUponCreation([Values(0, 2, 4)] int length)
    {
        var text = string.Join(
            "",
            _fixture.CreateMany<char>(1000)
                    .Where(c => char.IsBetween(c, 'A', 'Z') || char.IsBetween(c, 'a', 'z'))
                    .Take(length)
        );

        FluentActions.Invoking(() => new CurrencyCode(text))
                     .Should()
                     .Throw<Exception>()
                     .WithMessage("Invalid currency code.");
    }

    [Test]
    public void Given_3CharacterString_When_ContainsInvalidCharacters_Then_ThrowUponCreation()
    {
        var dodgyCharacter = _fixture.CreateMany<char>(100)
                                     .First(c => !(char.IsBetween(c, 'A', 'Z') || char.IsBetween(c, 'a', 'z')));
        var text = string.Join(
            "",
            _fixture.CreateMany<char>(1000)
                    .Where(c => char.IsBetween(c, 'A', 'Z') || char.IsBetween(c, 'a', 'z'))
                    .Take(2)
                    .Append(dodgyCharacter)
                    .Shuffle()
        );

        FluentActions.Invoking(() => new CurrencyCode(text))
                     .Should()
                     .Throw<Exception>()
                     .WithMessage("Invalid currency code.");
    }
}