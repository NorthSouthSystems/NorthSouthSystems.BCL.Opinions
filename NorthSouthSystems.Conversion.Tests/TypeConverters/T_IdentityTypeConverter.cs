public class T_IdentityTypeConverter : T_TypeConverter<IdentityTypeConverter>
{
    [Theory]
    [InlineData("", typeof(string))]
    [InlineData("foobar", typeof(string))]
    [InlineData(0, typeof(int))]
    [InlineData(1, typeof(int))]
    [InlineData(0, typeof(int?))]
    [InlineData(1, typeof(int?))]
    [InlineData(0.0, typeof(double))]
    [InlineData(1.0, typeof(double))]
    [InlineData(0.0, typeof(double?))]
    [InlineData(1.0, typeof(double?))]
    [InlineData(DayOfWeek.Monday, typeof(DayOfWeek))]
    [InlineData(DayOfWeek.Monday, typeof(DayOfWeek?))]
    public void IsConvertedTrue(object value, Type conversionType)
    {
        var request = Convert(value, conversionType);

        request.IsConverted.Should().BeTrue();
        request.ConvertedValue.Should().BeSameAs(value);
    }

    [Theory]
    [InlineData(null, typeof(string))]
    [InlineData(1, typeof(string))]
    [InlineData(null, typeof(int))]
    [InlineData(0.0, typeof(int))]
    [InlineData(1.0, typeof(int))]
    [InlineData(0.0, typeof(int?))]
    [InlineData(1.0, typeof(int?))]
    [InlineData(null, typeof(double))]
    [InlineData(0, typeof(double))]
    [InlineData(1, typeof(double))]
    [InlineData(null, typeof(double?))]
    [InlineData(0, typeof(double?))]
    [InlineData(1, typeof(double?))]
    [InlineData(null, typeof(DayOfWeek))]
    [InlineData(1, typeof(DayOfWeek))]
    [InlineData("Monday", typeof(DayOfWeek))]
    [InlineData(null, typeof(DayOfWeek?))]
    [InlineData(1, typeof(DayOfWeek?))]
    [InlineData("Monday", typeof(DayOfWeek?))]
    public void IsConvertedFalse(object value, Type conversionType)
    {
        var request = Convert(value, conversionType);

        request.IsConverted.Should().BeFalse();
        request.ConvertedValue.Should().BeNull();
    }
}