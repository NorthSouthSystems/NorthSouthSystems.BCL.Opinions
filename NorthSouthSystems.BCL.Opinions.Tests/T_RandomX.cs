public class T_RandomX
{
    [Fact]
    public void NextBool()
    {
        var random = new Random(8920);

        const int length = 1_000_000;

        bool[] bools = Enumerable.Range(0, length)
            .Select(_ => random.NextBool())
            .ToArray();

        int trueCount = bools.Count(b => b);
        decimal trueRatio = (decimal)trueCount / length;

        // This particular seed produced an incredibly "fair" distribution.
        Math.Abs(trueRatio - .5m).Should().BeLessThan(.00005m);
    }
}