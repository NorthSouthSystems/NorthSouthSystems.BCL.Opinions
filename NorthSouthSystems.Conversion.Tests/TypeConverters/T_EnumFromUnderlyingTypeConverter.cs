public class T_EnumFromUnderlyingTypeConverter : T_TypeConverter<EnumFromUnderlyingTypeConverter>
{
    // An Enum can be cast from any value that is in range of its UnderlyingType, hence the 8th day of the week test.
    public static IEnumerable<object[]> IsConvertedTrueData =>
        Enumerable.Range(0, 8)
            .SelectMany(i => new object[][]
            {
                [(byte)i, (DayOfWeek)i],
                [(sbyte)i, (DayOfWeek)i],
                [(ushort)i, (DayOfWeek)i],
                [(short)i, (DayOfWeek)i],
                [(uint)i, (DayOfWeek)i],
                [(int)i, (DayOfWeek)i],
                [(ulong)i, (DayOfWeek)i],
                [(long)i, (DayOfWeek)i]
            });

    [Theory]
    [MemberData(nameof(IsConvertedTrueData))]
    public void IsConvertedTrue(object value, object expectedValue)
    {
        Assert(Convert(value, typeof(DayOfWeek)));
        Assert(Convert(value, typeof(DayOfWeek?)));

        void Assert(ConvertTypeRequest request)
        {
            request.IsConverted.Should().BeTrue();
            request.ConvertedValue.Should().Be(expectedValue);
            request.ConvertedValue.GetType().Should().Be(typeof(DayOfWeek));
        }
    }

    [Theory]
    [InlineData("")]
    [InlineData("1")]
    [InlineData("Monday")]
    [InlineData(StringComparison.OrdinalIgnoreCase)]
    public void IsConvertedFalse(object value)
    {
        Assert(Convert(value, typeof(DayOfWeek)));
        Assert(Convert(value, typeof(DayOfWeek?)));

        static void Assert(ConvertTypeRequest request)
        {
            request.IsConverted.Should().BeFalse();
            request.ConvertedValue.Should().BeNull();
        }
    }
}