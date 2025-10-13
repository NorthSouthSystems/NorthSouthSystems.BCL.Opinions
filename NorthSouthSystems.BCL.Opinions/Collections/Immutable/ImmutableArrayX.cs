namespace NorthSouthSystems.Collections.Immutable;

using System.Collections.Immutable;

public static class ImmutableArrayX
{
    public static ImmutableArray<T> DefaultToEmpty<T>(this ImmutableArray<T> value) =>
        value.IsDefault ? [] : value;
}