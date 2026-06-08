namespace NorthSouthSystems.Conversion;

public class EnumFromUnderlyingTypeConverter : ITypeConverter
{
    public void Convert(ConvertTypeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        Type conversionType = request.ConversionType.FlattenGenericNullable();

        if (request.Value != null && request.Value.GetType().CanBeEnumUnderlyingType() && conversionType.IsEnum)
            request.Converted(Enum.ToObject(conversionType, request.Value));
    }
}