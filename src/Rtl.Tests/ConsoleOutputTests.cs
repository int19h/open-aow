using Xunit;

namespace Aow.Rtl.Tests;

public sealed class ConsoleOutputTests
{
    [Fact]
    public void WritesRawCodePage1252BytesAndCrLfWithoutTextWriterEncoding()
    {
        using var output = new MemoryStream();
        Rtl.ConsoleWrite(output, false, AnsiString.FromBytes(0, 128, 233, 255));
        Rtl.ConsoleWrite(output, true, " ", -12, " ", true, " ", false);
        Rtl.ConsoleWrite(output, true);
        Assert.Equal(new byte[] { 0, 128, 233, 255 }.Concat(" -12 TRUE FALSE\r\n\r\n"u8.ToArray()), output.ToArray());
    }

    [Fact]
    public void ConsoleFieldsFollowDcc32WidthsAndRealNotation()
    {
        Assert.Equal("   42", Rtl.ConsoleFormat(42, 5).ToString());
        Assert.Equal("  -42", Rtl.ConsoleFormat(-42, 5).ToString());
        Assert.Equal("    TRUE", Rtl.ConsoleFormat(true, 8).ToString());
        Assert.Equal("FALSE", Rtl.ConsoleFormat(false, 2).ToString());
        Assert.Equal("      1.25", Rtl.ConsoleFormat(1.25, 10, 2).ToString());
        Assert.Equal(" 1.25000000000000E+0000", Rtl.ConsoleFormat(1.25).ToString());
        Assert.Equal("x", Rtl.ConsoleFormat((AnsiString)"x", -4).ToString());
    }
}
