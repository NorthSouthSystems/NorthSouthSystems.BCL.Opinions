using System.Globalization;

public class T_ConvertX_Culture
{
    private static readonly ConvertX _convertX = new();

    [Fact]
    public void Culture()
    {
        // https://stackoverflow.com/a/8492527
        var negativeBangCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
        negativeBangCulture.NumberFormat.NegativeSign = "!";

        IsConvertedTrue("1", CultureInfo.InvariantCulture, 1);
        IsConvertedTrue("1", negativeBangCulture, 1);

        IsConvertedFalse("!1", CultureInfo.InvariantCulture);
        IsConvertedTrue("!1", negativeBangCulture, -1);

        IsConvertedTrue("-1", CultureInfo.InvariantCulture, -1);
        IsConvertedFalse("-1", negativeBangCulture);

        static void IsConvertedTrue(string value, CultureInfo culture, int expectedConvertedValue)
        {
            _convertX.ConvertType<int>(value, culture).Should().Be(expectedConvertedValue);
            _convertX.ConvertType(value, typeof(int), culture).Should().Be(expectedConvertedValue);

            int convertedValueInt;
            object convertedValueObject;

            _convertX.TryConvertType<int>(value, culture, out convertedValueInt).Should().BeTrue();
            convertedValueInt.Should().Be(expectedConvertedValue);

            _convertX.TryConvertType(value, typeof(int), culture, out convertedValueObject).Should().BeTrue();
            convertedValueObject.Should().Be(expectedConvertedValue);
        }

        static void IsConvertedFalse(string value, CultureInfo culture)
        {
            Action act;

            act = () => _convertX.ConvertType<int>(value, culture);
            act.Should().ThrowExactly<Exception>().WithInnerExceptionExactly<FormatException>();

            act = () => _convertX.ConvertType(value, typeof(int), culture);
            act.Should().ThrowExactly<Exception>().WithInnerExceptionExactly<FormatException>();

            int convertedValueInt;
            object convertedValueObject;

            _convertX.TryConvertType<int>(value, culture, out convertedValueInt).Should().BeFalse();
            convertedValueInt.Should().Be(0);

            _convertX.TryConvertType(value, typeof(int), culture, out convertedValueObject).Should().BeFalse();
            convertedValueObject.Should().Be(0);
        }
    }
}