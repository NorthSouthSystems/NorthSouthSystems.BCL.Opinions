using NorthSouthSystems.IO;
using System.Runtime.InteropServices;

public class T_PathX
{
    private static bool IsWindows => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

    [Fact]
    public void Basic()
    {
        if (IsWindows)
        {
            PathX.GetDirectoryNameOfCallerFilePath(@"C:\Foo\A.txt").Should().Be(@"C:\Foo");
            PathX.GetDirectoryNameOfCallerFilePath(@"C:\Foo\Bar\A.txt").Should().Be(@"C:\Foo\Bar");

            PathX.GetFullPathRelativeToCallerFilePath("B.txt", @"C:\Foo\A.txt").Should().Be(@"C:\Foo\B.txt");
            PathX.GetFullPathRelativeToCallerFilePath(@"Bar\B.txt", @"C:\Foo\A.txt").Should().Be(@"C:\Foo\Bar\B.txt");
        }
        else
        {
            PathX.GetDirectoryNameOfCallerFilePath("/Foo/A.txt").Should().Be("/Foo");
            PathX.GetDirectoryNameOfCallerFilePath("/Foo/Bar/A.txt").Should().Be("/Foo/Bar");

            PathX.GetFullPathRelativeToCallerFilePath("B.txt", "/Foo/A.txt").Should().Be("/Foo/B.txt");
            PathX.GetFullPathRelativeToCallerFilePath("Bar/B.txt", "/Foo/A.txt").Should().Be("/Foo/Bar/B.txt");
        }
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

        act = () => PathX.GetFullPathRelativeToCallerFilePath(IsWindows ? @"C:\Rooted.txt" : "/Rooted.txt");
        act.Should().ThrowExactly<ArgumentOutOfRangeException>();

        if (IsWindows)
        {
            act = () => PathX.GetDirectoryNameOfCallerFilePath(@"C:\");
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();

            act = () => PathX.GetFullPathRelativeToCallerFilePath("B.txt", @"C:\");
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
        else
        {
            act = () => PathX.GetDirectoryNameOfCallerFilePath("/");
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();

            act = () => PathX.GetFullPathRelativeToCallerFilePath("B.txt", "/");
            act.Should().ThrowExactly<ArgumentOutOfRangeException>();
        }
    }
}