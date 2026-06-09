using NorthSouthSystems.Reflection;
using System.Reflection;

[MemoryDiagnoser]
public class B_PropertyInfoX_GetValueCompiled
{
    [GlobalSetup]
    public void GlobalSetup()
    {
        _theIntPropertyCached ??= typeof(TheClass).GetProperty(nameof(TheClass.TheInt));
        _theStringPropertyCached ??= typeof(TheClass).GetProperty(nameof(TheClass.TheString));

        _theIntCompiledCached ??= PropertyInfoX.GetGetterCompiled(typeof(TheClass), nameof(TheClass.TheInt));
        _theStringCompiledCached ??= PropertyInfoX.GetGetterCompiled(typeof(TheClass), nameof(TheClass.TheString));
    }

    private static TheClass _theClass = new();

    private static PropertyInfo _theIntPropertyCached;
    private static PropertyInfo _theStringPropertyCached;

    private static Func<object, object> _theIntCompiledCached;
    private static Func<object, object> _theStringCompiledCached;

    public class TheClass
    {
        public int TheInt => 42;
        public string TheString => "foobar";
    }

    [Benchmark(Baseline = true)]
    public void Reflection()
    {
        typeof(TheClass).GetProperty(nameof(TheClass.TheInt)).GetValue(_theClass);
        typeof(TheClass).GetProperty(nameof(TheClass.TheString)).GetValue(_theClass);
    }

    [Benchmark]
    public void ReflectionCached()
    {
        _theIntPropertyCached.GetValue(_theClass);
        _theStringPropertyCached.GetValue(_theClass);
    }

    [Benchmark]
    public void GetValueCompiled()
    {
        PropertyInfoX.GetValueCompiled(_theClass, nameof(TheClass.TheInt));
        PropertyInfoX.GetValueCompiled(_theClass, nameof(TheClass.TheString));
    }

    [Benchmark]
    public void GetValueCompiledCached()
    {
        _theIntCompiledCached(_theClass);
        _theStringCompiledCached(_theClass);
    }
}