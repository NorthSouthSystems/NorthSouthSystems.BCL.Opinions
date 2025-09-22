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
    public void CurrentAndInvariant() => new T_StringX().CurrentAndInvariant();
}