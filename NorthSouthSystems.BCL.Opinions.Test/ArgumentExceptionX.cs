namespace NorthSouthSystems;

public class ArgumentExceptionXTests
{
    [Fact]
    public void ThrowIfAny()
    {
        string nl = Environment.NewLine;

        Action act;
        ArgumentException e;

        act = () => ArgumentExceptionX.ThrowIfAny(Array.Empty<string>());
        act.Should().NotThrow();

        act = () => ArgumentExceptionX.ThrowIfAny(["foo"]);
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("foo (Parameter");
        e.ParamName.Should().Be("[\"foo\"]");

        act = () => ArgumentExceptionX.ThrowIfAny(["foo", "bar"]);
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("foo" + nl + "bar (Parameter");
        e.ParamName.Should().Be("[\"foo\", \"bar\"]");

        act = () => ArgumentExceptionX.ThrowIfAny(["foo", "bar"], messagePrefix: "The prefix");
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("The prefix" + nl + "foo" + nl + "bar (Parameter");
        e.ParamName.Should().Be("[\"foo\", \"bar\"]");

        act = () => ArgumentExceptionX.ThrowIfAny(["foo", "bar"], messageIncludeIndices: true);
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("0: foo" + nl + "1: bar (Parameter");
        e.ParamName.Should().Be("[\"foo\", \"bar\"]");

        act = () => ArgumentExceptionX.ThrowIfAny(["foo", "bar"], originalParamName: "theParam");
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("foo" + nl + "bar (Parameter");
        e.ParamName.Should().Be("theParam");

        act = () => ArgumentExceptionX.ThrowIfAny(["foo", "bar"], "The prefix", true, "theParam");
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("The prefix" + nl + "0: foo" + nl + "1: bar (Parameter");
        e.ParamName.Should().Be("theParam");
    }
}