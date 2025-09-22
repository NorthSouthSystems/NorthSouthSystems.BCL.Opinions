namespace NorthSouthSystems.Reflection;

using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

// Adapted and simplified from a ChatGPT conversation on 2025-08-21.
public static class PropertyInfoX
{
    private static readonly ConcurrentDictionary<GetterCacheKey, Lazy<Func<object, object>>> _getterCache = new();
    private sealed record GetterCacheKey(Type ObjType, string PropertyPath, bool IncludeNonPublic);

    public static object GetValueCompiled(object obj, string propertyPath, bool includeNonPublic = false)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentException.ThrowIfNullOrEmpty(propertyPath);

        var objType = obj.GetType();
        ValidateObjType(objType);

        var cacheKey = new GetterCacheKey(objType, propertyPath, includeNonPublic);
        var getter = _getterCache.GetOrAdd(cacheKey, ck => new(() => CompileGetter(ck)));

        return getter.Value(obj);
    }

    public static Type GetGetterReturnTypeOrThrow(Type objType, string propertyPath, bool includeNonPublic = false)
    {
        ArgumentNullException.ThrowIfNull(objType);
        ArgumentException.ThrowIfNullOrEmpty(propertyPath);

        var propertyPathSegments = SplitPropertyPath(propertyPath);

        var currentType = objType;

        foreach (string propertyName in propertyPathSegments)
        {
            var getter = GetGetterMethodOrThrow(currentType, propertyName, includeNonPublic);

            currentType = getter.ReturnType;
        }

        return currentType;
    }

    public static MethodInfo GetGetterMethodOrThrow(Type objType, string propertyName, bool includeNonPublic = false)
    {
        ArgumentNullException.ThrowIfNull(objType);
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        var flattenFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        var declaredFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

        if (includeNonPublic)
        {
            flattenFlags |= BindingFlags.NonPublic;
            declaredFlags |= BindingFlags.NonPublic;
        }

        var property = objType.GetProperty(propertyName, flattenFlags)
            ?? throw new ArgumentException($"Property '{propertyName}' not found on {objType} with the requested accessibility.", nameof(propertyName));

        // We must get the PropertyInfo from the DeclaringType or else Expression.Property will throw:
        // "System.ArgumentException : The method '...' is not a property accessor (Parameter 'propertyAccessor')"
        if (property.DeclaringType != objType && property.DeclaringType is not null)
            property = property.DeclaringType.GetProperty(propertyName, declaredFlags)!;

        return property.GetGetMethod(includeNonPublic)
            ?? throw new ArgumentException($"Property '{propertyName}' on {objType} does not have a getter with the requested accessibility.", nameof(propertyName));
    }

    private static void ValidateObjType(Type objType)
    {
        if (objType.IsValueType)
            throw new ArgumentException("Value types are not allowed.", nameof(objType));
    }

    private static Func<object, object> CompileGetter(GetterCacheKey key)
    {
        var propertyPathSegments = SplitPropertyPath(key.PropertyPath);

        var objParameter = Expression.Parameter(typeof(object));

        Expression currentValue = Expression.Convert(objParameter, key.ObjType);
        var currentValueType = key.ObjType;

        foreach (string propertyName in propertyPathSegments)
        {
            var getter = GetGetterMethodOrThrow(currentValueType, propertyName, key.IncludeNonPublic);

            currentValue = Expression.Property(currentValue, getter);
            currentValueType = getter.ReturnType;
        }

        if (currentValueType.IsValueType)
            currentValue = Expression.Convert(currentValue, typeof(object));

        var lambda = Expression.Lambda<Func<object, object>>(currentValue, objParameter);

        return lambda.Compile();
    }

    private static string[] SplitPropertyPath(string propertyPath) =>
        propertyPath.Split(['.'], StringSplitOptions.TrimEntries);
}