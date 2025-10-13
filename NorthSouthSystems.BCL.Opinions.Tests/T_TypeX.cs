using System.Collections.Immutable;

public class T_TypeX
{
    [Fact]
    public void Default()
    {
        typeof(int).Default().Should().Be(default(int));
        typeof(DateTime).Default().Should().Be(default(DateTime));
        typeof(ImmutableArray<int>).Default().Should().Be(default(ImmutableArray<int>));

        typeof(object).Default().Should().Be(default(object));
        typeof(string).Default().Should().Be(default(string));
    }

    [Fact]
    public void IsFloatingPoint()
    {
        typeof(double).IsFloatingPoint().Should().BeTrue();
        typeof(int).IsFloatingPoint().Should().BeFalse();
    }

    [Fact]
    public void IsIntegral()
    {
        typeof(double).IsIntegral().Should().BeFalse();
        typeof(int).IsIntegral().Should().BeTrue();
    }

    [Fact]
    public void CSharpKeywordsByType()
    {
        TypeX.CSharpKeywordsByType[typeof(double)].Should().Be("double");
        TypeX.CSharpKeywordsByType[typeof(int)].Should().Be("int");

        TypeX.CSharpKeywordsByType.ContainsKey(typeof(TypeX)).Should().BeFalse();
    }
}