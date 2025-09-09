namespace NorthSouthSystems;

public class TypeXTests
{
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