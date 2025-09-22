public class T_ObjectX
{
    [Fact]
    public void DeferredNew()
    {
        List<string> strings = null;

        ObjectX.DeferredNew(ref strings).Add("foo");
        strings.Should().Equal(["foo"]);

        ObjectX.DeferredNew(ref strings).Add("bar");
        strings.Should().Equal(["foo", "bar"]);
    }
}