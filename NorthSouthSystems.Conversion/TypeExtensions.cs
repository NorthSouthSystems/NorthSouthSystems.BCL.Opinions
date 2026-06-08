namespace NorthSouthSystems.Conversion;

internal static class TypeExtensions
{
    internal static object? GetDefaultValue(this Type type) =>
        (!type.IsValueType || IsGenericNullable(type))
            ? null
            : Activator.CreateInstance(type);

    internal static bool CanBeEnumUnderlyingType(this Type type) => _enumUnderlyingTypes.Contains(type);

    private static readonly HashSet<Type> _enumUnderlyingTypes = new(
        [typeof(byte), typeof(sbyte), typeof(ushort), typeof(short), typeof(uint), typeof(int), typeof(ulong), typeof(long)]);

    internal static bool IsGenericNullable(this Type type) => Nullable.GetUnderlyingType(type) != null;
    internal static Type FlattenGenericNullable(this Type type) => Nullable.GetUnderlyingType(type) ?? type;
}