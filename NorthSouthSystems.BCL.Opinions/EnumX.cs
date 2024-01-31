namespace NorthSouthSystems;

public static class EnumX
{
    public static IEnumerable<TEnum> GetValues<TEnum>()
            where TEnum : Enum =>
        Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
}