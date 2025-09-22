// Adapted and simplified from a ChatGPT conversation on 2025-08-21.
using NorthSouthSystems.Reflection;

public class T_PropertyInfoX
{
    public sealed class TheClass
    {
        public string StringProperty => "foobar";
        public int IntProperty => 43;
        public double? NullableDoubleProperty => 1.23;

        public string NullStringProperty => null;
        public double? NullDoubleProperty => null;

        public OtherClass Other => new();

        // For Exceptions.
        internal string NonPublicStringProperty => "nonpublic";
        public string SetterOnlyStringProperty { set { } }
    }

    public sealed class OtherClass
    {
        public string StringProperty => "FUBAR";
        public int IntProperty => 34;
        public double? NullableDoubleProperty => 3.21;

        public string NullStringProperty => null;
        public double? NullDoubleProperty => null;

        public AnotherClass Another => new();
    }

    public sealed class AnotherClass
    {
        public string StringProperty => "woot";
    }

    public class TheBase
    {
        public string StringProperty => "base";

        public OtherClass Other => new();
    }

    public sealed class TheDerived : TheBase
    {
        public AnotherClass Another => new();
    }

    // For Exceptions.
    public struct TheStruct { public string StringProperty => "struct"; }

    [Fact]
    public void GetValueComiled_SinglePart()
    {
        object result;

        result = PropertyInfoX.GetValueCompiled(new TheClass(), nameof(TheClass.StringProperty));
        result.Should().BeOfType<string>().Which.Should().Be(new TheClass().StringProperty);

        result = PropertyInfoX.GetValueCompiled(new TheClass(), nameof(TheClass.IntProperty));
        result.Should().BeOfType<int>().Which.Should().Be(new TheClass().IntProperty);

        result = PropertyInfoX.GetValueCompiled(new TheClass(), nameof(TheClass.NullableDoubleProperty));
        result.Should().BeOfType<double>().Which.Should().Be(new TheClass().NullableDoubleProperty);

        result = PropertyInfoX.GetValueCompiled(new TheClass(), nameof(TheClass.NullStringProperty));
        result.Should().BeNull();

        result = PropertyInfoX.GetValueCompiled(new TheClass(), nameof(TheClass.NullDoubleProperty));
        result.Should().BeNull();

        // Derived

        result = PropertyInfoX.GetValueCompiled(new TheDerived(), nameof(TheDerived.StringProperty));
        result.Should().BeOfType<string>().Which.Should().Be(new TheDerived().StringProperty);
    }

    [Fact]
    public void GetValueComiled_TwoPart()
    {
        object result;

        result = PropertyInfoX.GetValueCompiled(new TheClass(), $"{nameof(TheClass.Other)}.{nameof(OtherClass.StringProperty)}");
        result.Should().BeOfType<string>().Which.Should().Be(new OtherClass().StringProperty);

        result = PropertyInfoX.GetValueCompiled(new TheClass(), $"{nameof(TheClass.Other)}.{nameof(OtherClass.IntProperty)}");
        result.Should().BeOfType<int>().Which.Should().Be(new OtherClass().IntProperty);

        result = PropertyInfoX.GetValueCompiled(new TheClass(), $"{nameof(TheClass.Other)}.{nameof(OtherClass.NullableDoubleProperty)}");
        result.Should().BeOfType<double>().Which.Should().Be(new OtherClass().NullableDoubleProperty);

        result = PropertyInfoX.GetValueCompiled(new TheClass(), $"{nameof(TheClass.Other)}.{nameof(OtherClass.NullStringProperty)}");
        result.Should().BeNull();

        result = PropertyInfoX.GetValueCompiled(new TheClass(), $"{nameof(TheClass.Other)}.{nameof(OtherClass.NullDoubleProperty)}");
        result.Should().BeNull();

        // Derived

        result = PropertyInfoX.GetValueCompiled(new TheDerived(), $"{nameof(TheDerived.Other)}.{nameof(OtherClass.StringProperty)}");
        result.Should().BeOfType<string>().Which.Should().Be(new OtherClass().StringProperty);

        result = PropertyInfoX.GetValueCompiled(new TheDerived(), $"{nameof(TheDerived.Another)}.{nameof(AnotherClass.StringProperty)}");
        result.Should().BeOfType<string>().Which.Should().Be(new AnotherClass().StringProperty);
    }

    [Fact]
    public void GetValueComiled_ThreePart()
    {
        object result;

        result = PropertyInfoX.GetValueCompiled(new TheClass(), $"{nameof(TheClass.Other)}.{nameof(OtherClass.Another)}.{nameof(AnotherClass.StringProperty)}");
        result.Should().BeOfType<string>().Which.Should().Be(new AnotherClass().StringProperty);

        // Derived

        result = PropertyInfoX.GetValueCompiled(new TheDerived(), $"{nameof(TheDerived.Other)}.{nameof(OtherClass.Another)}.{nameof(AnotherClass.StringProperty)}");
        result.Should().BeOfType<string>().Which.Should().Be(new AnotherClass().StringProperty);
    }

    [Fact]
    public void GetValueCompiled_Exceptions()
    {
        Action act;

        act = static () => PropertyInfoX.GetValueCompiled(null, nameof(TheClass.StringProperty));
        act.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().Be("obj");

        act = static () => PropertyInfoX.GetValueCompiled(new TheClass(), null);
        act.Should().ThrowExactly<ArgumentNullException>().Which.ParamName.Should().Be("propertyPath");

        act = static () => PropertyInfoX.GetValueCompiled(new TheClass(), string.Empty);
        act.Should().ThrowExactly<ArgumentException>().Which.ParamName.Should().Be("propertyPath");

        act = static () => PropertyInfoX.GetValueCompiled(new TheStruct(), nameof(TheStruct.StringProperty));
        act.Should().ThrowExactly<ArgumentException>().WithMessage("Value types are not allowed*");
    }

    [Fact]
    public void GetGetterMethodOrThrow_Exceptions()
    {
        Action act;

        act = static () => PropertyInfoX.GetValueCompiled(new TheClass(), "DoesNotExist");
        act.Should().ThrowExactly<ArgumentException>();

        act = static () => PropertyInfoX.GetValueCompiled(new TheClass(), nameof(TheClass.NonPublicStringProperty));
        act.Should().ThrowExactly<ArgumentException>();

        act = static () => PropertyInfoX.GetValueCompiled(new TheClass(), nameof(TheClass.SetterOnlyStringProperty));
        act.Should().ThrowExactly<ArgumentException>();
    }
}