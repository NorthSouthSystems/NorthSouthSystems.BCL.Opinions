namespace NorthSouthSystems;

public static class RandomX
{
    public static bool NextBool(this Random random) =>
        random == null
            ? throw new ArgumentNullException(nameof(random))
            : random.Next(2) == 1;
}