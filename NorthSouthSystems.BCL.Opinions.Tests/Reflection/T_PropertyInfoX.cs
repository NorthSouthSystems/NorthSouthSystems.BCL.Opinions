// Adapted and simplified from a ChatGPT conversation on 2025-08-21.
using NorthSouthSystems.Reflection;

public class T_PropertyInfoX
{
    private sealed class InputClass
    {
        public string StringProperty => "foobar";
        public int IntProperty => 43;
        public double? NullableDoubleProperty => 1.23;

        public string NullStringProperty => null;
        public double? NullDoubleProperty => null;

        internal string NonPublicStringProperty => "nonpublic";
        public string SetterOnlyStringProperty { set { } }
    }

    private sealed class InputClassDerived : InputClassBase { }

    protected class InputClassBase { public string StringProperty => "base"; }

    private struct InputStruct { public string StringProperty => "struct"; }

    [Fact]
    public void GetValueComiled()
    {
        object result;

        result = PropertyInfoX.GetValueCompiled(new InputClass(), nameof(InputClass.StringProperty));
        result.Should().BeOfType<string>().Which.Should().Be(new InputClass().StringProperty);

        result = PropertyInfoX.GetValueCompiled(new InputClass(), nameof(InputClass.IntProperty));
        result.Should().BeOfType<int>().Which.Should().Be(new InputClass().IntProperty);

        result = PropertyInfoX.GetValueCompiled(new InputClass(), nameof(InputClass.NullableDoubleProperty));
        result.Should().BeOfType<double>().Which.Should().Be(new InputClass().NullableDoubleProperty);

        result = PropertyInfoX.GetValueCompiled(new InputClass(), nameof(InputClass.NullStringProperty));
        result.Should().BeNull();

        result = PropertyInfoX.GetValueCompiled(new InputClass(), nameof(InputClass.NullDoubleProperty));
        result.Should().BeNull();

        result = PropertyInfoX.GetValueCompiled(new InputClassDerived(), nameof(InputClassDerived.StringProperty));
        result.Should().BeOfType<string>().Which.Should().Be(new InputClassDerived().StringProperty);
    }

    [Fact]
    public void GetValueCompiled_Exceptions()
    {
        Action act;

        act = static () => PropertyInfoX.GetValueCompiled(null, nameof(InputClass.StringProperty));
        act.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().Be("obj");

        act = static () => PropertyInfoX.GetValueCompiled(new InputClass(), null);
        act.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().Be("propertyName");

        act = static () => PropertyInfoX.GetValueCompiled(new InputClass(), string.Empty);
        act.Should().ThrowExactly<ArgumentException>().Which.ParamName.Should().Be("propertyName");

        act = static () => PropertyInfoX.GetValueCompiled(new InputStruct(), nameof(InputStruct.StringProperty));
        act.Should().ThrowExactly<ArgumentException>().WithMessage("Value types are not allowed*");
    }

    [Fact]
    public void GetGetterMethodOrThrow_Exceptions()
    {
        Action act;

        act = static () => PropertyInfoX.GetValueCompiled(new InputClass(), "DoesNotExist");
        act.Should().ThrowExactly<ArgumentException>();

        act = static () => PropertyInfoX.GetValueCompiled(new InputClass(), nameof(InputClass.NonPublicStringProperty));
        act.Should().ThrowExactly<ArgumentException>();

        act = static () => PropertyInfoX.GetValueCompiled(new InputClass(), nameof(InputClass.SetterOnlyStringProperty));
        act.Should().ThrowExactly<ArgumentException>();
    }
}