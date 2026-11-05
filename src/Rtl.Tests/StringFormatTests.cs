using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class StringFormatTests
{
    private static void Same(string format, params FormatArg[] args)
    {
        string ansi = format;
        var error = Record.Exception(() => Rtl.Format(ansi, args));
        if (error is null) Assert.Equal(Rtl.Format(ansi, args), Rtl.Format(format, args));
        else Assert.Equal(Assert.IsType<EConvertError>(error).Message,
            Assert.Throws<EConvertError>(() => Rtl.Format(format, args)).Message);
    }

    [Fact]
    public void FormatMatchesEveryFormatProbeInput()
    {
        Same("%s", "abc"); Same("%d", -123); Same("%x", 255); Same("%x", -1);
        Same("%6s", "abc"); Same("%6d", -12); Same("%6x", 255);
        Same("%.2s", "abcd"); Same("%.5d", -12); Same("%.6x", 255);
        Same("%-6s", "abc"); Same("%-6.4d", 12); Same("%*d", 6, 12);
        Same("%.*s", 2, "abcd"); Same("%*.*x", 8, 4, 255);
        Same("%1:s/%0:d/%1:s", 7, "abc"); Same("%1:d/%d/%0:d", 10, 20, 30);
        Same("%1:*.*s", 99, 6, 2, "abcd"); Same("%% %d %%", 5);
        Same("%3s", ""); Same("%.0s", "abc"); Same("%.0d", 0); Same("%d", 1, 2);
        Same("%d", "abc"); Same("%s %d", "abc"); Same("%q", 1); Same("%2:d", 1);
        Same("%*d", "bad", 1);
    }

    [Fact]
    public void FormatMatchesTaggedKindsAndParserEdges()
    {
        foreach (string format in new[] { "%", "%3", "%-", "%.", "%0:", "%*", "%.*", "abc%" }) Same(format);
        Same("%.16d", 7); Same("%.15d", 7); Same("%.0s", (byte)'A');
        Same("%u", -1); Same("%D", int.MinValue); Same("%X", int.MinValue);
        Same("%p", new PointerArg(uint.MaxValue)); Same("%10.2P", new PointerArg(1));
        Same("%s", Rtl.StrPas(Ptr<byte>.Unbox("abc\0def"))); Same("%s", "a\0b");
        Same("%*:*s", 1, 2, "bc"); Same("%*d", false, 1); Same("%*d", "x", 1);
        Same("%0:1:s", 7, "abcdef"); Same("%:s", "abc");
        foreach (string kind in new[] { "%e", "%f", "%g", "%n", "%m" }) Same(kind, 1.25);
        foreach (FormatArg arg in new FormatArg[] { true, 1.25, new TObject(), (TObject?)null, new PointerArg(0) }) Same("%s", arg);
        Same("%p", 1); Same("%d", (byte)'a'); Same("%0:1:d", 7, "abcdef");
        Assert.Equal("A", Rtl.Format("%.0s", 'A'));
        Assert.Equal("abc", Rtl.Format("%s", "abc\0def"[..3]));
        Assert.Equal("€\0Ÿ", Rtl.Format("€\0%s", 'Ÿ'));
    }

    private sealed class SampleObject : TObject;

    // An exhaustive switch over the cases. A case added to FormatArg fails this with CS8509.
    private static string CaseOf(FormatArg argument) => argument switch
    {
        int value => "Integer " + value.ToString(CultureInfo.InvariantCulture),
        bool value => "Boolean " + value,
        char value => $"Char U+{(int)value:X4}",
        double value => "Extended " + value.ToString(CultureInfo.InvariantCulture),
        string value => "String " + value,
        TObject value => "Object " + value.GetType().Name,
        PointerArg value => $"Pointer {value.Value:X8}",
        null => "null",
    };

    [Fact]
    public void EveryConversionSelectsItsCase()
    {
        using var mode = new StringMode(false);
        // Integers. A constant that fits a byte stays an Integer, a narrower integer widens to one.
        Assert.Equal("Integer 7", CaseOf(7));
        Assert.Equal("Integer 1", CaseOf(1));
        Assert.Equal("Integer -2147483648", CaseOf(int.MinValue));
        Assert.Equal("Integer -2", CaseOf((short)-2));
        Assert.Equal("Integer -3", CaseOf((sbyte)-3));
        Assert.Equal("Integer 65535", CaseOf((ushort)65535));
        Assert.Equal("Integer 65", CaseOf((int)(byte)65));
        // A Delphi Char is a byte and enters through the frozen map.
        Assert.Equal("Char U+0041", CaseOf((byte)65));
        Assert.Equal("Char U+20AC", CaseOf((byte)0x80));
        Assert.Equal("Char U+0081", CaseOf((byte)0x81));
        Assert.Equal("Char U+0178", CaseOf((byte)0x9F));
        Assert.Equal("Char U+00FF", CaseOf((byte)0xFF));
        byte delphiChar = 0x80;
        Assert.Equal("Char U+20AC", CaseOf(delphiChar));
        // A string element is a char and keeps its value.
        Assert.Equal("Char U+0041", CaseOf('A'));
        Assert.Equal("Char U+20AC", CaseOf('€'));
        Assert.Equal("Char U+0080", CaseOf('\u0080'));
        Assert.Equal("Char U+0178", CaseOf("xŸ"[1]));
        // Booleans, floating values and the unsigned and long integers, which widen to Extended.
        Assert.Equal("Boolean True", CaseOf(true));
        Assert.Equal("Boolean False", CaseOf(false));
        Assert.Equal("Extended 1.25", CaseOf(1.25));
        Assert.Equal("Extended 1.5", CaseOf(1.5f));
        Assert.Equal("Extended 9", CaseOf(9u));
        Assert.Equal("Extended 11", CaseOf(11L));
        // Strings. A nil string reads as empty outside strict mode.
        Assert.Equal("String abc", CaseOf("abc"));
        Assert.Equal("String ", CaseOf(""));
        Assert.Equal("String ", CaseOf((string?)null));
        // Objects. A nil object is the union's null, like the default value.
        Assert.Equal("Object TObject", CaseOf(new TObject()));
        Assert.Equal("Object SampleObject", CaseOf(new SampleObject()));
        Assert.Equal("null", CaseOf((TObject?)null));
        Assert.Equal("null", CaseOf(default));
        Assert.Equal("Pointer FFFFFFFF", CaseOf(new PointerArg(uint.MaxValue)));
        // The same conversions in a collection expression and in a params span.
        var obj = new SampleObject();
        FormatArg[] list = [7, (byte)0x80, '\u0080', true, 1.25, "s", obj, (TObject?)null, new PointerArg(1)];
        Assert.Equal(new[] { "Integer 7", "Char U+20AC", "Char U+0080", "Boolean True", "Extended 1.25", "String s",
            "Object SampleObject", "null", "Pointer 00000001" }, list.Select(CaseOf));
        Assert.Equal("7 € € \u0080 x", Rtl.Format("%d %s %s %s %s", 7, (byte)0x80, '€', '\u0080', "x"));
        Assert.Equal("7 € x", Rtl.Format("%d %s %s", [7, (byte)0x80, "x"]));
    }

    [Fact]
    public void EveryCaseHoldsItsValue()
    {
        var obj = new TObject();
        Assert.Equal(7, ((FormatArg)7).Value);
        Assert.Equal('€', ((FormatArg)(byte)0x80).Value);
        Assert.Equal('\u0080', ((FormatArg)'\u0080').Value);
        Assert.True(((FormatArg)true).Value is true);
        Assert.Equal(1.25, ((FormatArg)1.25).Value);
        Assert.Equal("abc", ((FormatArg)"abc").Value);
        Assert.Same(obj, ((FormatArg)obj).Value);
        Assert.Equal(new PointerArg(3), ((FormatArg)new PointerArg(3)).Value);
        Assert.Null(((FormatArg)(TObject?)null).Value);
        Assert.Null(default(FormatArg).Value);
        Assert.True(((FormatArg)0).HasValue);
        Assert.True(((FormatArg)false).HasValue);
        Assert.True(((FormatArg)"").HasValue);
        Assert.False(((FormatArg)(TObject?)null).HasValue);
        Assert.False(default(FormatArg).HasValue);
        Assert.True(((FormatArg)int.MinValue).TryGetValue(out int integer) && integer == int.MinValue);
        Assert.False(((FormatArg)int.MinValue).TryGetValue(out char _));
        Assert.True(((FormatArg)(-0.0)).TryGetValue(out double negativeZero) && double.IsNegative(negativeZero));
        Assert.False(((FormatArg)'A').TryGetValue(out int _));
        Assert.False(((FormatArg)"A").TryGetValue(out char _));
    }

    [Fact]
    public void TheConversionsAreTheCasesAndTheDelphiChar()
    {
        var type = typeof(FormatArg);
        Assert.True(type.IsValueType);
        Assert.True(typeof(IUnion).IsAssignableFrom(type));
        Assert.True(type.IsDefined(typeof(UnionAttribute)));
        string[] cases = type.GetConstructors().Select(c => c.GetParameters()).Where(p => p.Length == 1)
            .Select(p => p[0].ParameterType.Name).Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(new[] { "Boolean", "Char", "Double", "Int32", "PointerArg", "String", "TObject" }, cases);
        string[] conversions = type.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == "op_Implicit")
            .Select(m => m.GetParameters()[0].ParameterType.Name).Order(StringComparer.Ordinal).ToArray();
        Assert.Equal(new[] { "Boolean", "Byte", "Char", "Double", "Int32", "PointerArg", "String", "TObject" }, conversions);
    }

    public static TheoryData<string, string, string> Conversions()
    {
        // Each row: an argument, a conversion and the result or the error message.
        const string integer = "Integer format argument required", text = "String format argument required";
        const string pointer = "Pointer format argument required", unsupported = "Unsupported format at 1";
        var data = new TheoryData<string, string, string>();
        string[][] table =
        [
            ["int", "-7", "4294967289", "FFFFFFF9", text, pointer],
            ["byte", integer, integer, integer, "€", pointer],
            ["char", integer, integer, integer, "€", pointer],
            ["char80", integer, integer, integer, "\u0080", pointer],
            ["bool", integer, integer, integer, text, pointer],
            ["double", integer, integer, integer, text, pointer],
            ["string", integer, integer, integer, "abc", pointer],
            ["object", integer, integer, integer, text, pointer],
            ["nil", integer, integer, integer, text, pointer],
            ["default", integer, integer, integer, text, pointer],
            ["pointer", integer, integer, integer, text, "0000BEEF"],
        ];
        string[] letters = ["d", "u", "x", "s", "p"];
        foreach (var row in table)
        {
            for (int i = 0; i < letters.Length; i++)
            {
                data.Add(row[0], "%" + letters[i], row[i + 1]);
                data.Add(row[0], "%" + letters[i].ToUpperInvariant(), row[i + 1]);
            }
            foreach (string other in new[] { "%e", "%f", "%g", "%n", "%m", "%q" }) data.Add(row[0], other, unsupported);
        }
        return data;
    }

    private static FormatArg Argument(string name) => name switch
    {
        "int" => -7,
        "byte" => (byte)0x80,
        "char" => '€',
        "char80" => '\u0080',
        "bool" => true,
        "double" => 1.25,
        "string" => "abc",
        "object" => new TObject(),
        "nil" => (TObject?)null,
        "default" => default,
        "pointer" => new PointerArg(0xBEEF),
        _ => throw new ArgumentOutOfRangeException(nameof(name)),
    };

    [Theory]
    [MemberData(nameof(Conversions))]
    public void EveryCaseMeetsEveryConversionCharacter(string argument, string format, string expected)
    {
        FormatArg value = Argument(argument);
        string actual;
        try { actual = Rtl.Format(format, value); }
        catch (EConvertError error) { actual = error.Message; }
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void OnlyAnIntegerSetsAStarWidth()
    {
        foreach (string name in new[] { "byte", "char", "char80", "bool", "double", "string", "object", "nil", "default", "pointer" })
            Assert.Equal("1", Rtl.Format("%*d", Argument(name), 1));
        Assert.Equal("    1", Rtl.Format("%*d", 5, 1));
        Assert.Equal("1    ", Rtl.Format("%-*d", 5, 1));
        Assert.Equal("ab", Rtl.Format("%.*s", 2, "abc"));
        // A Delphi Char byte is no count, so the precision is 0.
        Assert.Equal("", Rtl.Format("%.*s", (byte)2, "abc"));
        // Precision truncates a string and leaves a Char whole, from a byte or a char.
        Assert.Equal("€", Rtl.Format("%.0s", (byte)0x80));
        Assert.Equal("€", Rtl.Format("%.0s", '€'));
        Assert.Equal("", Rtl.Format("%.0s", "€"));
    }
}
