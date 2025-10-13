using NorthSouthSystems.Collections.Immutable;
using System.Collections.Immutable;

public class T_ImmutableArrayX
{
    [Fact]
    public void DefaultToEmpty()
    {
        ImmutableArray<int> immArr = default;
        immArr.IsDefault.Should().Be(true);

        immArr = immArr.DefaultToEmpty();
        immArr.IsDefault.Should().BeFalse();
        immArr.Should().Equal(ImmutableArray<int>.Empty);

        immArr = [43];
        immArr = immArr.DefaultToEmpty();
        immArr.IsDefault.Should().BeFalse();
        immArr.Should().Equal([43]);
    }
}