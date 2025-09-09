namespace NorthSouthSystems;

using NorthSouthSystems.Runtime.CompilerServices;
using System.Runtime.CompilerServices;

public static class StringX
{
    public static string Current(ref DefaultInterpolatedStringHandler handler)
        => handler.ToStringAndClear();

    public static string Invariant(ref InvariantInterpolatedStringHandler handler)
        => handler.ToStringAndClear();
}