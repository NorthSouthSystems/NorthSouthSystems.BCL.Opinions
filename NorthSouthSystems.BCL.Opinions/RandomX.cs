namespace NorthSouthSystems;

public static class RandomX
{
#pragma warning disable CA5394 // We are explicitly extending Random rather than a secure RNG.
    public static bool NextBool(this Random random) =>
        random == null
            ? throw new ArgumentNullException(nameof(random))
            : random.Next(2) == 1;
#pragma warning restore
}