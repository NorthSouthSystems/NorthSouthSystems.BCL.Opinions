public class T_ConvertX_ArgumentExceptions
{
    [Fact]
    public void ArgumentExceptions()
    {
        Action act;

        act = () => new ConvertX((IEnumerable<ITypeConverter>)null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new ConvertX(new IdentityTypeConverter(), null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new ConvertX(Array.Empty<ITypeConverter>());
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();

        act = () => new ConvertX().ConvertType("", null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => new ConvertX().TryConvertType("", null, out var _);
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}