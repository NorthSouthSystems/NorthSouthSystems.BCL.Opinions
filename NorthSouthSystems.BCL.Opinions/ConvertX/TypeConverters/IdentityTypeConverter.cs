namespace NorthSouthSystems.Conversion;

public class IdentityTypeConverter : ITypeConverter
{
    public void Convert(ConvertTypeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        // All Nullable<T> instances box as their UnderlyingType.
        if (request.Value?.GetType() == request.ConversionType.FlattenGenericNullable())
            request.Converted(request.Value);
    }
}