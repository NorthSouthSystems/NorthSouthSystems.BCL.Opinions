using NorthSouthSystems.Linq;

public class T_ReverseNoBufferExtensions
{
    [Fact]
    public void Basic()
    {
        Assert(Array.Empty<int>());
        Assert([1]);
        Assert([1, 2]);
        Assert([2, 1]);
        Assert([1, 1]);
        Assert([1, 2, 3]);
        Assert([1, 3, 2]);

        static void Assert<T>(IReadOnlyList<T> source) =>
            source.ReverseNoBuffer().Should().Equal(source.Reverse());
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => ReverseNoBufferExtensions.ReverseNoBuffer((IReadOnlyList<string>)null);
        act.Should().ThrowExactly<ArgumentNullException>();
    }
}