using System.Globalization;

public abstract class T_TypeConverter<TTypeConverter>
    where TTypeConverter : ITypeConverter, new()
{
    private readonly TTypeConverter _typeConverter = new();

    protected ConvertTypeRequest Convert(object value, Type conversionType, IFormatProvider provider = null)
    {
        var request = new ConvertTypeRequest(value, conversionType, provider ?? CultureInfo.CurrentCulture);
        _typeConverter.Convert(request);

        return request;
    }
}