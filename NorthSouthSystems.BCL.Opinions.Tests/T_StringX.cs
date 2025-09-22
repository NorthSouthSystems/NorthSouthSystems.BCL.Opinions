using System.Globalization;

public class T_StringX
{
    [Fact]
    public void CurrentAndInvariant()
    {
        decimal currency = 1_234.56m;

        WithCulture("en-US", () =>
        {
            StringX.Current($"{currency:C2}").Should().Be("$1,234.56");
            StringX.Invariant($"{currency:C2}").Should().Be("¤1,234.56");
        });

        WithCulture("de-DE", () =>
        {
            StringX.Current($"{currency:C2}").Should().Be("1.234,56 €");
            StringX.Invariant($"{currency:C2}").Should().Be("¤1,234.56");
        });
    }

    private static void WithCulture(string name, Action action)
    {
        var currentCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name);
            action();
        }
        finally
        {
            CultureInfo.CurrentCulture = currentCulture;
        }
    }
}