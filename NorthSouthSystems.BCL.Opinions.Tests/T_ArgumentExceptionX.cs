public class T_ArgumentExceptionX
{
    [Fact]
    public void ThrowIfAny()
    {
        string nl = Environment.NewLine;

        Action act;
        ArgumentException e;

        act = () => ArgumentExceptionX.ThrowIfAny((IEnumerable<string>)null);
        act.Should().NotThrow();

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

    [Fact]
    public void ThrowIfDefault()
    {
        Action act;
        ArgumentException e;

        act = () => ArgumentExceptionX.ThrowIfDefault((int?)1);
        act.Should().NotThrow();

        act = () => ArgumentExceptionX.ThrowIfDefault(1);
        act.Should().NotThrow();

        act = () => ArgumentExceptionX.ThrowIfDefault(DateTime.Now);
        act.Should().NotThrow();

        int? iNullable = null;

        act = () => ArgumentExceptionX.ThrowIfDefault(iNullable);
        e = act.Should().ThrowExactly<ArgumentNullException>().Which;
        e.Message.Should().StartWith("Value cannot be null.");
        e.ParamName.Should().Be(nameof(iNullable));

        iNullable = 0;

        act = () => ArgumentExceptionX.ThrowIfDefault(iNullable);
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("Value cannot be default.");
        e.ParamName.Should().Be(nameof(iNullable));

        int i = default;

        act = () => ArgumentExceptionX.ThrowIfDefault(i);
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("Value cannot be default.");
        e.ParamName.Should().Be(nameof(i));

        DateTime dt = default;

        act = () => ArgumentExceptionX.ThrowIfDefault(dt);
        e = act.Should().ThrowExactly<ArgumentException>().Which;
        e.Message.Should().StartWith("Value cannot be default.");
        e.ParamName.Should().Be(nameof(dt));
    }
}