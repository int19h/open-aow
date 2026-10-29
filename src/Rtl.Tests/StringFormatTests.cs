using Assert = Xunit.Assert;

namespace Aow.Rtl.Tests;

public sealed class StringFormatTests
{
    private static void Same(string format, params TVarRec[] args)
    {
        AnsiString ansi = format;
        var error = Record.Exception(() => Rtl.Format(ansi, args));
        if (error is null) Assert.Equal(Rtl.Format(ansi, args).ToString(), Rtl.Format(format, args));
        else Assert.Equal(Assert.IsType<EConvertError>(error).Message.ToString(),
            Assert.Throws<EConvertError>(() => Rtl.Format(format, args)).Message.ToString());
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
        Same("%p", TVarRec.Pointer(uint.MaxValue)); Same("%10.2P", TVarRec.Pointer(1));
        Same("%s", ((AnsiString)"abc\0def").AsPtr()); Same("%s", "a\0b");
        Same("%*:*s", 1, 2, "bc"); Same("%*d", false, 1); Same("%*d", "x", 1);
        Same("%0:1:s", 7, "abcdef"); Same("%:s", "abc");
        foreach (string kind in new[] { "%e", "%f", "%g", "%n", "%m" }) Same(kind, 1.25);
        foreach (TVarRec arg in new TVarRec[] { true, 1.25, new TObject(), (TObject.Meta?)null, TVarRec.Pointer(0) }) Same("%s", arg);
        Same("%p", 1); Same("%d", (byte)'a'); Same("%0:1:d", 7, "abcdef");
        Assert.Equal("A", Rtl.Format("%.0s", 'A'));
        Assert.Equal("abc", Rtl.Format("%s", "abc\0def"[..3]));
        Assert.Equal("\u20ac\0\u0178", Rtl.Format("\u20ac\0%s", '\u0178'));
    }
}
