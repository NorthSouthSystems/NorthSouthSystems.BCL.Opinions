using NorthSouthSystems.Globalization;

public class T_CultureInfoX
{
    [Fact]
    public void Exceptions()
    {
        Action act;

        act = static () => CultureInfoX.WithCulture("en-US", null);
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void WithCulture()
    {
        string before = CurrencyToString();

        CultureInfoX.WithCulture("en-US", () => CurrencyToString().Should().Be("$1,234.56"));
        CurrencyToString().Should().Be(before);

        CultureInfoX.WithCulture("de-DE", () => CurrencyToString().Should().Be("1.234,56 €"));
        CurrencyToString().Should().Be(before);
        return;

        static string CurrencyToString()
        {
            const decimal currency = 1_234.56m;

            return $"{currency:C2}";
        }
    }
}