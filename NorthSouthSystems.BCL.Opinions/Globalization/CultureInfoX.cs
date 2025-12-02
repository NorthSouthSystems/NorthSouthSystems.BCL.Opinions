using System.Globalization;

namespace NorthSouthSystems.Globalization;

public static class CultureInfoX
{
    public static void WithCulture(string name, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        var currentCulture = CultureInfo.CurrentCulture;

        try
        {
            CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(name);
            action();
        }
        finally
        {
            CultureInfo.CurrentCulture = currentCulture;
        }
    }
}