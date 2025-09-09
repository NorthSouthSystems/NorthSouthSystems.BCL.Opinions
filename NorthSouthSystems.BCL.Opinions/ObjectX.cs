namespace NorthSouthSystems;

public static class ObjectX
{
    public static T DeferredNew<T>(ref T? obj)
        where T : class, new()
    {
        return obj ??= new();
    }
}