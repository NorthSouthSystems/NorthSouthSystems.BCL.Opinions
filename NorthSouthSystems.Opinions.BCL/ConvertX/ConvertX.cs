using System.Globalization;

namespace NorthSouthSystems;

public interface ITypeConverter
{
    void Convert(ConvertTypeRequest request);
}

/// <summary>
/// ConvertX is used to compose pipelines of ITypeConverters capable of converting .NET objects of one Type to any other
/// Type. The default ConvertX conversion pipeline properly handles empty strings and Nullable of T Types for which
/// System.Convert throws Exceptions.
/// </summary>
public class ConvertX
{
    // Construction

    public static IEnumerable<ITypeConverter> DefaultTypeConverters { get; } =
    [
        new IdentityTypeConverter(),
        new NullTypeConverter(),
        new StringEmptyTypeConverter(),
        new EnumFromUnderlyingTypeConverter(),
        new SystemConvertTypeConverter()
    ];

    public ConvertX()
        : this(DefaultTypeConverters)
    { }

    public ConvertX(params ITypeConverter[] typeConverters)
        : this((IEnumerable<ITypeConverter>)typeConverters)
    { }

    public ConvertX(IEnumerable<ITypeConverter> typeConverters)
    {
        // Always "make a copy" of the enumerable in case it is a modifiable collection.
        _typeConverters = typeConverters?.ToArray() ?? throw new ArgumentNullException(nameof(typeConverters));

        if (_typeConverters.Any(tc => tc == null))
            throw new ArgumentNullException(nameof(typeConverters));

        if (!_typeConverters.Any())
            throw new ArgumentOutOfRangeException(nameof(typeConverters));
    }

    private readonly IEnumerable<ITypeConverter> _typeConverters;

    // ConvertType Generic and Object

    public TConversionType? ConvertType<TConversionType>(object? value,
        IFormatProvider? provider = null, bool throwIntermediateExceptions = false)
    {
        var request = ConvertTypeImpl(value, typeof(TConversionType), provider, throwIntermediateExceptions, false);

        return request.IsConverted ? (TConversionType?)request.ConvertedValue : throw request.ExceptionToThrow();
    }

    public object? ConvertType(object? value, Type conversionType,
        IFormatProvider? provider = null, bool throwIntermediateExceptions = false)
    {
        var request = ConvertTypeImpl(value, conversionType, provider, throwIntermediateExceptions, false);

        return request.IsConverted ? request.ConvertedValue : throw request.ExceptionToThrow();
    }

    // TryConvertType Generic

    public bool TryConvertType<TConversionType>(object? value,
        out TConversionType? convertedValue) =>
        TryConvertType(value, null, false, out convertedValue);

    public bool TryConvertType<TConversionType>(object? value,
        IFormatProvider? provider, out TConversionType? convertedValue) =>
        TryConvertType(value, provider, false, out convertedValue);

    public bool TryConvertType<TConversionType>(object? value,
        bool abortIntermediateExceptions, out TConversionType? convertedValue) =>
        TryConvertType(value, null, abortIntermediateExceptions, out convertedValue);

    public bool TryConvertType<TConversionType>(object? value,
        IFormatProvider? provider, bool abortIntermediateExceptions, out TConversionType? convertedValue)
    {
        var request = ConvertTypeImpl(value, typeof(TConversionType), provider, false, abortIntermediateExceptions);

        convertedValue = request.IsConverted ? (TConversionType?)request.ConvertedValue : default;

        return request.IsConverted;
    }

    // TryConvertType Object

    public bool TryConvertType(object? value, Type conversionType,
        out object? convertedValue) =>
        TryConvertType(value, conversionType, null, false, out convertedValue);

    public bool TryConvertType(object? value, Type conversionType,
        IFormatProvider? provider, out object? convertedValue) =>
        TryConvertType(value, conversionType, provider, false, out convertedValue);

    public bool TryConvertType(object? value, Type conversionType,
        bool abortIntermediateExceptions, out object? convertedValue) =>
        TryConvertType(value, conversionType, null, abortIntermediateExceptions, out convertedValue);

    public bool TryConvertType(object? value, Type conversionType,
        IFormatProvider? provider, bool abortIntermediateExceptions, out object? convertedValue)
    {
        var request = ConvertTypeImpl(value, conversionType, provider, false, abortIntermediateExceptions);

        // We want to have complete parity with TryConvertType<TConversionType>, so whenever conversion didn't occur,
        // we use our extension method to "default" convertedValue instead of simply always setting null.
        convertedValue = request.IsConverted ? request.ConvertedValue : conversionType.Default();

        return request.IsConverted;
    }

    // Implementation

    private ConvertTypeRequest ConvertTypeImpl(object? value, Type conversionType,
        IFormatProvider? provider, bool throwIntermediateExceptions, bool abortIntermediateExceptions)
    {
        var request = new ConvertTypeRequest(value, conversionType, provider ?? CultureInfo.CurrentCulture);

        foreach (var typeConverter in _typeConverters)
        {
            try
            {
                typeConverter.Convert(request);

                if (request.IsConverted)
                    return request;
            }
            catch (Exception exception)
            {
                if (throwIntermediateExceptions)
                    throw;

                request.Exception(exception);

                if (abortIntermediateExceptions)
                    return request;
            }
        }

        return request;
    }
}