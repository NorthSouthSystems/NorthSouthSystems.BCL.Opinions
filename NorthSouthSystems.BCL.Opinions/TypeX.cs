﻿namespace NorthSouthSystems;

using System.Collections.Immutable;
using System.Numerics;
using System.Runtime.CompilerServices;

public static class TypeX
{
    public static object? Default(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return type.IsValueType
            ? RuntimeHelpers.GetUninitializedObject(type)
            : null;
    }

    // Unfortunately, there is no simpler method to determine this. All Systems.Numerics interfaces
    // are recursive generics (i.e. IInterface<T> where T : IInterface<T>), so they can't be used
    // with "is" or "as" operators on instances or with Type.IsAssignable for Types (without Reflection).
    public static bool IsFloatingPoint(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return FloatingPointTypes.Contains(type);
    }

    public static ImmutableHashSet<Type> FloatingPointTypes { get; } =
    [
        typeof(Half),
        typeof(float),
        typeof(double),
        typeof(decimal)
    ];

    // Unfortunately, there is no simpler method to determine this. All Systems.Numerics interfaces
    // are recursive generics (i.e. IInterface<T> where T : IInterface<T>), so they can't be used
    // with "is" or "as" operators on instances or with Type.IsAssignable for Types (without Reflection).
    public static bool IsIntegral(this Type type)
    {
        ArgumentNullException.ThrowIfNull(type);

        return IntegralTypes.Contains(type);
    }

    public static ImmutableHashSet<Type> IntegralTypes { get; } =
    [
        typeof(byte),
        typeof(sbyte),
        typeof(short),
        typeof(ushort),
        typeof(int),
        typeof(uint),
        typeof(long),
        typeof(ulong),

        typeof(Int128),
        typeof(BigInteger)
    ];

    public static ImmutableDictionary<Type, string> CSharpKeywordsByType { get; } =
        new Dictionary<Type, string>
        {
            [typeof(bool)] = "bool",
            [typeof(byte)] = "byte",
            [typeof(sbyte)] = "sbyte",
            [typeof(short)] = "short",
            [typeof(ushort)] = "ushort",
            [typeof(int)] = "int",
            [typeof(uint)] = "uint",
            [typeof(long)] = "long",
            [typeof(ulong)] = "ulong",
            [typeof(float)] = "float",
            [typeof(double)] = "double",
            [typeof(decimal)] = "decimal",

            [typeof(char)] = "char",
            [typeof(string)] = "string",

            [typeof(object)] = "object",
        }
        .ToImmutableDictionary();
}