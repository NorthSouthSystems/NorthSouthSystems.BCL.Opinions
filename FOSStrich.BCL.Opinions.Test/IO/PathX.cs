namespace FOSStrich.IO;

public class PathXTests
{
    [Fact]
    public void Basic()
    {
        PathX.GetDirectoryNameOfCallerFilePath(@"C:\Foo\A.txt").Should().Be(@"C:\Foo");
        PathX.GetDirectoryNameOfCallerFilePath(@"C:\Foo\Bar\A.txt").Should().Be(@"C:\Foo\Bar");

        PathX.GetFullPathRelativeToCallerFilePath("B.txt", @"C:\Foo\A.txt").Should().Be(@"C:\Foo\B.txt");
        PathX.GetFullPathRelativeToCallerFilePath(@"Bar\B.txt", @"C:\Foo\A.txt").Should().Be(@"C:\Foo\Bar\B.txt");
    }

    [Fact]
    public void Exceptions()
    {
        Action act;

        act = () => PathX.GetDirectoryNameOfCallerFilePath(null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => PathX.GetDirectoryNameOfCallerFilePath(@"NotRooted.txt");
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();

        act = () => PathX.GetFullPathRelativeToCallerFilePath(null);
        act.Should().ThrowExactly<ArgumentNullException>();

        act = () => PathX.GetFullPathRelativeToCallerFilePath(@"C:\Rooted.txt");
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();
    }
}