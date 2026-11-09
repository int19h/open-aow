using System.Reflection;
using System.Runtime.CompilerServices;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

// Brief 9.6-4: the message slot union, checked the way StringFormatTests checks FormatArg.
public sealed class MsgArgTests
{
    private sealed class SampleObject;

    private sealed class OtherObject;

    // An exhaustive switch over the cases. A case added to MsgArg fails this with CS8509.
    private static string CaseOf(MsgArg argument) => argument switch
    {
        int value => "Integer " + value,
        object value => "Object " + value.GetType().Name,
        null => "null",
    };

    private static string[] Cases(params ReadOnlySpan<MsgArg> arguments)
    {
        var result = new string[arguments.Length];
        for (int i = 0; i < arguments.Length; i++) result[i] = CaseOf(arguments[i]);
        return result;
    }

    [Fact]
    public void EveryConversionSelectsItsCase()
    {
        Assert.Equal("Integer 7", CaseOf(7));
        Assert.Equal("Integer 0", CaseOf(0));
        Assert.Equal("Integer -2147483648", CaseOf(int.MinValue));
        // A narrower integer widens to the integer case.
        Assert.Equal("Integer -2", CaseOf((short)-2));
        Assert.Equal("Integer -3", CaseOf((sbyte)-3));
        Assert.Equal("Integer 255", CaseOf((byte)255));
        Assert.Equal("Integer 65535", CaseOf((ushort)65535));
        // A reference keeps its runtime type, a nil reference is the union's null.
        Assert.Equal("Object SampleObject", CaseOf(new SampleObject()));
        Assert.Equal("Object Object", CaseOf(new object()));
        Assert.Equal("null", CaseOf((object?)null));
        Assert.Equal("null", CaseOf(default));
        // The same conversions in a collection expression and in a params span.
        var obj = new SampleObject();
        MsgArg[] list = [7, obj, (object?)null, (byte)1];
        Assert.Equal(new[] { "Integer 7", "Object SampleObject", "null", "Integer 1" }, list.Select(CaseOf));
        Assert.Equal(new[] { "Integer 7", "Object SampleObject", "null" }, Cases(7, obj, (object?)null));
    }

    [Fact]
    public void EveryCaseHoldsItsValue()
    {
        var obj = new object();
        Assert.Equal(7, ((MsgArg)7).Value);
        Assert.Same(obj, new MsgArg(obj).Value);
        Assert.Null(new MsgArg((object?)null).Value);
        Assert.Null(default(MsgArg).Value);
        Assert.True(((MsgArg)0).HasValue);
        Assert.True(new MsgArg(obj).HasValue);
        Assert.False(new MsgArg((object?)null).HasValue);
        Assert.False(default(MsgArg).HasValue);
        Assert.True(((MsgArg)int.MinValue).TryGetValue(out int integer) && integer == int.MinValue);
        Assert.False(((MsgArg)int.MinValue).TryGetValue(out object? _));
        Assert.True(new MsgArg(obj).TryGetValue(out object? reference) && ReferenceEquals(reference, obj));
        Assert.False(new MsgArg(obj).TryGetValue(out int none) || none != 0);
        Assert.False(default(MsgArg).TryGetValue(out int _));
        Assert.False(default(MsgArg).TryGetValue(out object? _));
    }

    [Fact]
    public void IntReadsTheIntegerAndThrowsOnAReference()
    {
        Assert.Equal(-5, ((MsgArg)(-5)).Int);
        Assert.Equal(0, ((MsgArg)0).Int);
        Assert.Equal(0, default(MsgArg).Int);
        Assert.Equal(0, new MsgArg((object?)null).Int);
        // A reference no longer offers its address as an integer.
        var obj = new SampleObject();
        Assert.Throws<InvalidCastException>(() => ((MsgArg)obj).Int);
    }

    [Fact]
    public void AsReadsTheReferenceAndIsNullOtherwise()
    {
        var obj = new SampleObject();
        Assert.Same(obj, ((MsgArg)obj).As<SampleObject>());
        Assert.Same(obj, ((MsgArg)obj).As<object>());
        Assert.Null(((MsgArg)obj).As<OtherObject>());
        Assert.Null(((MsgArg)7).As<object>());
        Assert.Null(((MsgArg)0).As<object>());
        Assert.Null(default(MsgArg).As<object>());
        Assert.Null(new MsgArg((object?)null).As<SampleObject>());
    }

    [Fact]
    public void EqualityIsTheEqualityOfTheOriginalWord()
    {
        var obj = new SampleObject();
        var other = new SampleObject();
        MsgArg empty = default, zero = 0, nil = (object?)null, one = 1, reference = obj;
        // Empty, the integer 0 and nil are all the word 0.
        Assert.True(empty == zero);
        Assert.True(zero == nil);
        Assert.True(nil == empty);
        Assert.Equal(empty.GetHashCode(), zero.GetHashCode());
        // An integer equals the same integer only.
        Assert.True(one == (MsgArg)1);
        Assert.True(one != zero);
        Assert.True(one != (MsgArg)2);
        // A reference equals the same object only, and never an integer.
        Assert.True(reference == (MsgArg)obj);
        Assert.True(reference != (MsgArg)other);
        Assert.True(reference != empty);
        Assert.True(reference != (MsgArg)RuntimeHelpers.GetHashCode(obj));
        // A direct Equals call prefers the MsgArg overload and wraps a reference. The override is the boxed call.
        object boxed = Box(reference);
        Assert.True(boxed.Equals(Box(reference)));
        Assert.False(boxed.Equals(obj));
    }

    private static object Box<T>(T value) where T : struct => value;

    [Fact]
    public void TheConversionsAreTheCases()
    {
        var type = typeof(MsgArg);
        Assert.True(type.IsValueType);
        Assert.True(typeof(IUnion).IsAssignableFrom(type));
        Assert.True(type.IsDefined(typeof(UnionAttribute)));
        string[] cases = type.GetConstructors().Select(c => c.GetParameters()).Where(p => p.Length == 1)
            .Select(p => p[0].ParameterType.Name).Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(new[] { "Int32", "Object" }, cases);
        string[] conversions = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == "op_Implicit")
            .Select(m => m.GetParameters()[0].ParameterType.Name).Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(new[] { "Int32" }, conversions);
    }
}
