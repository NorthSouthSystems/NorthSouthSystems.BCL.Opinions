using System.Collections.Immutable;

namespace NorthSouthSystems.Collections.Immutable;

public static class ImmutableArrayX
{
    public static ImmutableArray<T> DefaultToEmpty<T>(this ImmutableArray<T> value) =>
        value.IsDefault ? [] : value;
}