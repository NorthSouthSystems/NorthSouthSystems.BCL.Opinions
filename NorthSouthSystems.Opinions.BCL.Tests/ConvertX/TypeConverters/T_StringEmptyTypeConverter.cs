public class T_StringEmptyTypeConverter : T_TypeConverter<StringEmptyTypeConverter>
{
    [Fact]
    public void IsConvertedTrueNoOp()
    {
        var request = Convert(string.Empty, typeof(string));

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.As<string>().Should().BeEmpty();
    }

    [Theory]
    [InlineData(typeof(int?))]
    [InlineData(typeof(DayOfWeek?))]
    [InlineData(typeof(ITypeConverter))]
    public void IsConvertedTrueToNull(Type conversionType)
    {
        var request = Convert(string.Empty, conversionType);

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.Should().BeNull();
    }

    [Theory]
    [InlineData(typeof(int))]
    [InlineData(typeof(DayOfWeek))]
    public void IsConvertedFalse(Type conversionType)
    {
        var request = Convert(null, conversionType);

        request.IsConverted.Should().BeFalse();
        request.ConvertedValue.Should().BeNull();
    }
}