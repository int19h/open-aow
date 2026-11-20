
namespace Aow.Rtl.Tests;

public sealed class ConsoleOutputTests
{
    [Fact]
    public void WritesRawCodePage1252BytesAndCrLfWithoutTextWriterEncoding()
    {
        using MemoryStream output = new();
        ConsoleOutput.ConsoleWrite(output, newline: false, "\0\u20AC\u00E9\u00FF");
        ConsoleOutput.ConsoleWrite(output, newline: true, " ", -12, " ", true, " ", false);
        ConsoleOutput.ConsoleWrite(output, newline: true);
        Assert.Equal(new byte[] { 0, 128, 233, 255 }.Concat(" -12 TRUE FALSE\r\n\r\n"u8.ToArray()), output.ToArray());
    }

    [Fact]
    public void ConsoleFieldsFollowDcc32WidthsAndRealNotation()
    {
        Assert.Equal("   42", ConsoleOutput.ConsoleFormat(42, width: 5));
        Assert.Equal("  -42", ConsoleOutput.ConsoleFormat(-42, width: 5));
        Assert.Equal("    TRUE", ConsoleOutput.ConsoleFormat(true, width: 8));
        Assert.Equal("FALSE", ConsoleOutput.ConsoleFormat(false, width: 2));
        Assert.Equal("      1.25", ConsoleOutput.ConsoleFormat(1.25, width: 10, precision: 2));
        Assert.Equal(" 1.25000000000000E+0000", ConsoleOutput.ConsoleFormat(1.25));
        Assert.Equal("x", ConsoleOutput.ConsoleFormat("x", width: -4));
    }
}
