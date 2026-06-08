public class T_SystemConvertConverter : T_TypeConverter<SystemConvertTypeConverter>
{
    [Theory]
    [InlineData("true", typeof(bool), true)]
    [InlineData("1", typeof(int), 1)]
    [InlineData("1", typeof(double), 1.0)]
    public void IsConvertedTrue(object value, Type conversionType, object expectedConvertedValue)
    {
        var request = Convert(value, conversionType);

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.Should().Be(expectedConvertedValue);
    }
}