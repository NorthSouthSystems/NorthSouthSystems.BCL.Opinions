using System.Globalization;

public class T_ConvertX
{
    private static readonly ConvertX _convertX = new();

    [Theory]
    // NoOpTypeConverter
    [InlineData("foobar", typeof(string), "foobar")]
    [InlineData(1, typeof(int), 1)]
    // NullTypeConverter
    [InlineData(null, typeof(string), null)]
    [InlineData(null, typeof(int?), null)]
    // StringEmptyTypeConverter
    [InlineData("", typeof(int?), null)]
    // EnumFromUnderlyingTypeConverter
    [InlineData(1, typeof(DayOfWeek), DayOfWeek.Monday)]
    // SystemConvertTypeConverter
    [InlineData("true", typeof(bool), true)]
    public void IsConvertedTrue(object value, Type conversionType, object expectedConvertedValue)
    {
        _convertX.ConvertType(value, conversionType).Should().Be(expectedConvertedValue);
        _convertX.ConvertType(value, conversionType, CultureInfo.CurrentCulture).Should().Be(expectedConvertedValue);
        _convertX.ConvertType(value, conversionType, CultureInfo.InvariantCulture).Should().Be(expectedConvertedValue);

        object convertedValue;

        _convertX.TryConvertType(value, conversionType, out convertedValue).Should().BeTrue();
        convertedValue.Should().Be(expectedConvertedValue);

        _convertX.TryConvertType(value, conversionType, CultureInfo.CurrentCulture, out convertedValue).Should().BeTrue();
        convertedValue.Should().Be(expectedConvertedValue);

        _convertX.TryConvertType(value, conversionType, CultureInfo.InvariantCulture, out convertedValue).Should().BeTrue();
        convertedValue.Should().Be(expectedConvertedValue);
    }

    [Fact]
    public void IsConvertedFalseNotSupported()
    {
        Action act;
        object value = new();

        act = () => _convertX.ConvertType<ConvertX>(value);
        act.Should().ThrowExactly<NotSupportedException>();

        act = () => _convertX.ConvertType(value, typeof(ConvertX));
        act.Should().ThrowExactly<NotSupportedException>();

        _convertX.TryConvertType<ConvertX>(value, out ConvertX convertedValueConvertX).Should().BeFalse();
        convertedValueConvertX.Should().BeNull();

        _convertX.TryConvertType(value, typeof(ConvertX), out object convertedValueObject).Should().BeFalse();
        convertedValueObject.Should().BeNull();
    }

    [Fact]
    public void IsConvertedFalseSystemConvertTypeConverterFormatException()
    {
        Action act;
        string value = "foobar";

        act = () => _convertX.ConvertType<int>(value);
        act.Should().ThrowExactly<Exception>().WithInnerExceptionExactly<FormatException>();

        act = () => _convertX.ConvertType<int>(value, throwIntermediateExceptions: true);
        act.Should().ThrowExactly<FormatException>();

        act = () => _convertX.ConvertType(value, typeof(int));
        act.Should().ThrowExactly<Exception>().WithInnerExceptionExactly<FormatException>();

        act = () => _convertX.ConvertType(value, typeof(int), throwIntermediateExceptions: true);
        act.Should().ThrowExactly<FormatException>();

        _convertX.TryConvertType<int>(value, out int convertedValueInt).Should().BeFalse();
        convertedValueInt.Should().Be(0);

        _convertX.TryConvertType(value, typeof(int), out object convertedValueObject).Should().BeFalse();
        convertedValueObject.Should().Be(0);
    }

    [Fact]
    public void IsConvertedFalseSystemConvertTypeConverterFormatExceptionTwice()
    {
        // DO NOT USE _convertX within this method!
        //
        // This is technically not using the "DefaultTypeConverters" because there is no way to generate an
        // AggregateException when doing so. To generate an AggregateException, we have SystemConvertTypeConverter
        // execute twice.
        var convertX = new ConvertX(ConvertX.DefaultTypeConverters.Append(new SystemConvertTypeConverter()));

        Action act;
        string value = "foobar";

        act = () => convertX.ConvertType<int>(value);
        act.Should().ThrowExactly<AggregateException>().And.InnerExceptions.Select(ie => ie.GetType()).Should().Equal(typeof(FormatException), typeof(FormatException));

        act = () => convertX.ConvertType<int>(value, throwIntermediateExceptions: true);
        act.Should().ThrowExactly<FormatException>();

        act = () => convertX.ConvertType(value, typeof(int));
        act.Should().ThrowExactly<AggregateException>().And.InnerExceptions.Select(ie => ie.GetType()).Should().Equal(typeof(FormatException), typeof(FormatException));

        act = () => convertX.ConvertType(value, typeof(int), throwIntermediateExceptions: true);
        act.Should().ThrowExactly<FormatException>();

        convertX.TryConvertType<int>(value, out int convertedValueInt).Should().BeFalse();
        convertedValueInt.Should().Be(0);

        convertX.TryConvertType(value, typeof(int), out object convertedValueObject).Should().BeFalse();
        convertedValueObject.Should().Be(0);
    }

    [Fact]
    public void IntermediateExceptions()
    {
        // DO NOT USE _convertX within this method!
        //
        // This is technically not using the "DefaultTypeConverters" because there is no way to detect an
        // intermediate Exception in the TryConvertType methods when doing so. To generate a TryConvertType
        // detectable intermediate Exception, we append a converter that will always succeed.
        var convertX = new ConvertX(ConvertX.DefaultTypeConverters.Append(new AlwaysDefaultConverter()));

        Action act;
        string value = "foobar";

        convertX.ConvertType<int>(value).Should().Be(0);

        act = () => convertX.ConvertType<int>(value, throwIntermediateExceptions: true);
        act.Should().ThrowExactly<FormatException>();

        convertX.ConvertType(value, typeof(int)).Should().Be(0);

        act = () => convertX.ConvertType(value, typeof(int), throwIntermediateExceptions: true);
        act.Should().ThrowExactly<FormatException>();

        int outInt;
        object outObject;

        convertX.TryConvertType<int>(value, out outInt).Should().BeTrue();
        outInt.Should().Be(0);

        convertX.TryConvertType<int>(value, abortIntermediateExceptions: true, out outInt).Should().BeFalse();
        outInt.Should().Be(0);

        convertX.TryConvertType(value, typeof(int), out outObject).Should().BeTrue();
        outObject.Should().Be(0);

        convertX.TryConvertType(value, typeof(int), abortIntermediateExceptions: true, out outObject).Should().BeFalse();
        outObject.Should().Be(0);
    }

    private class AlwaysDefaultConverter : ITypeConverter
    {
        public void Convert(ConvertTypeRequest request) => request.Converted(request.ConversionType.GetDefaultValue());
    }
}