
namespace Aow.Rtl.Tests;

public sealed class ConsoleOutputTests
{
    [Fact]
    public void WritesRawCodePage1252BytesAndCrLfWithoutTextWriterEncoding()
    {
        using MemoryStream output = new();
        ConsoleOutput.ConsoleWrite(output, false, "\0\u20AC\u00E9\u00FF");
        ConsoleOutput.ConsoleWrite(output, true, " ", -12, " ", true, " ", false);
        ConsoleOutput.ConsoleWrite(output, true);
        Assert.Equal(new byte[] { 0, 128, 233, 255 }.Concat(" -12 TRUE FALSE\r\n\r\n"u8.ToArray()), output.ToArray());
    }

    [Fact]
    public void ConsoleFieldsFollowDcc32WidthsAndRealNotation()
    {
        Assert.Equal("   42", ConsoleOutput.ConsoleFormat(42, 5));
        Assert.Equal("  -42", ConsoleOutput.ConsoleFormat(-42, 5));
        Assert.Equal("    TRUE", ConsoleOutput.ConsoleFormat(true, 8));
        Assert.Equal("FALSE", ConsoleOutput.ConsoleFormat(false, 2));
        Assert.Equal("      1.25", ConsoleOutput.ConsoleFormat(1.25, 10, 2));
        Assert.Equal(" 1.25000000000000E+0000", ConsoleOutput.ConsoleFormat(1.25));
        Assert.Equal("x", ConsoleOutput.ConsoleFormat("x", -4));
    }
}
