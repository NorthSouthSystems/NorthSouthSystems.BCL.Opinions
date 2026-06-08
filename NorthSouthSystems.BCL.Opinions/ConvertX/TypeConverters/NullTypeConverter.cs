namespace NorthSouthSystems.Conversion;

public class NullTypeConverter : ITypeConverter
{
    public void Convert(ConvertTypeRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        if (request.Value == null && request.ConversionTypeAllowsNull)
            request.Converted(null);
    }
}