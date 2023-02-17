using AutoFixture.AutoFakeItEasy;
using AutoFixture.Kernel;
using MarketDataGateway.DataTypes;
using static MoreLinq.MoreEnumerable;

namespace MarketDataGatewayTests;

public static class AutoFixtureExtensions
{
    public static IFixture WithDataTypes(this IFixture self)
    {
        self.Customize<CurrencyCode>(
            x => x.FromFactory(
                (ISpecimenBuilder sb) => new CurrencyCode(CreateAlphaString(3, sb))
            )
        );

        return self;

        string CreateAlphaString(int length, ISpecimenBuilder specimenBuilder)
        {
            return string.Join(
                "", Generate(CreateLetter(specimenBuilder), _ => CreateLetter(specimenBuilder)).Take(length)
            );
        }

        char CreateLetter(ISpecimenBuilder sb)
        {
            return (char)('A' + sb.Create<int>() % 26);
        }
    }

    public static IFixture WithFakeItEasy(this IFixture self)
    {
        return self.Customize(new AutoFakeItEasyCustomization());
    }
}