namespace NorthSouthSystems.Reflection;

using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

// Adapted and simplified from a ChatGPT conversation on 2025-08-21.
public static class PropertyInfoX
{
    private static readonly ConcurrentDictionary<GetterCacheKey, Lazy<Func<object, object>>> _getterCache = new();
    private sealed record GetterCacheKey(Type ObjType, string PropertyPath);

    public static object GetValueCompiled(object obj, string propertyPath)
    {
        ArgumentNullException.ThrowIfNull(obj);
        ArgumentException.ThrowIfNullOrEmpty(propertyPath);

        var objType = obj.GetType();
        ValidateObjType(objType);

        var cacheKey = new GetterCacheKey(objType, propertyPath);
        var getter = _getterCache.GetOrAdd(cacheKey, ck => new(() => CompileGetter(ck)));

        return getter.Value(obj);
    }

    public static MethodInfo GetGetterMethodOrThrow(Type objType, string propertyName)
    {
        ValidateObjType(objType);
        ArgumentException.ThrowIfNullOrEmpty(propertyName);

        const BindingFlags flattenFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy;
        const BindingFlags declaredFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;

        var property = objType.GetProperty(propertyName, flattenFlags)
            ?? throw new ArgumentException($"Public property '{propertyName}' not found on {objType}.", nameof(propertyName));

        // We must get the PropertyInfo from the DeclaringType or else Expression.Property will throw:
        // "System.ArgumentException : The method '...' is not a property accessor (Parameter 'propertyAccessor')"
        if (property.DeclaringType != objType && property.DeclaringType is not null)
            property = property.DeclaringType.GetProperty(propertyName, declaredFlags)!;

        return property.GetGetMethod()
            ?? throw new ArgumentException($"Property '{propertyName}' on {objType} does not have a public getter.", nameof(propertyName));
    }

    private static void ValidateObjType(Type objType)
    {
        if (objType.IsValueType)
            throw new ArgumentException("Value types are not allowed.", nameof(objType));
    }

    private static Func<object, object> CompileGetter(GetterCacheKey key)
    {
        var propertyPathSegments = key.PropertyPath.Split(['.'], StringSplitOptions.TrimEntries);

        var objParameter = Expression.Parameter(typeof(object));

        Expression currentValue = Expression.Convert(objParameter, key.ObjType);
        var currentValueType = key.ObjType;

        foreach (string propertyName in propertyPathSegments)
        {
            var getter = GetGetterMethodOrThrow(currentValueType, propertyName);

            currentValue = Expression.Property(currentValue, getter);
            currentValueType = getter.ReturnType;
        }

        if (currentValueType.IsValueType)
            currentValue = Expression.Convert(currentValue, typeof(object));

        var lambda = Expression.Lambda<Func<object, object>>(currentValue, objParameter);

        return lambda.Compile();
    }
}