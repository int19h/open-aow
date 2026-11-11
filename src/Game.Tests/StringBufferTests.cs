using Aow.Engine.Storage;

using Aow.Game.Draw.Images;
using Aow.Game.Ui.Controls;
using Aow.Game.Ui.Windows;

namespace Aow.Game.Tests;

public sealed class StringBufferTests
{
    private static void Read(int shape, ref string text, Stream stream)
    {
        if (shape == 0) GameEngine.StringFromStream(ref text, stream);
        else if (shape == 1) EReadStorageStream.LoadStringFromStream(ref text, stream);
        else LibraryDrawing.ReadStringFromStream(ref text, stream);
    }

    private static MemoryStream Input(int shape, int length, ReadOnlySpan<byte> bytes)
    {
        var stream = new MemoryStream();
        if (shape == 1) stream.Write([255]);
        stream.Write(Bytes.Of(ref length));
        stream.Write(bytes);
        stream.Position = 0;
        return stream;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void StringReadersKeepEveryByteAndIndependentAliases(int shape)
    {
        byte[] bytes = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
        using var stream = Input(shape, bytes.Length, bytes);
        string text = "old", alias = text;
        Read(shape, ref text, stream);
        Assert.Equal(bytes, Cp1252.Encode(text).ToArray());
        Assert.Equal("old", alias);
        Assert.Equal(stream.Length, stream.Position);
    }

    [Fact]
    public void ShortStringFromStreamKeepsTheResizedTail()
    {
        using var stream = Input(0, 6, [0, 0x80, 0xFF]);
        string text = "seedtail";
        GameEngine.StringFromStream(ref text, stream);
        Assert.Equal(new byte[] { 0, 0x80, 0xFF, 100, 116, 97 }, Cp1252.Encode(text).ToArray());
    }

    [Theory]
    [InlineData(1)]
    [InlineData(2)]
    public void ShortStringBodiesThrowAndKeepTheResize(int shape)
    {
        using var stream = Input(shape, 6, [0, 0x80, 0xFF]);
        string text = "seedtail";
        Assert.Throws<EndOfStreamException>(() => Read(shape, ref text, stream));
        Assert.Equal(shape == 1 ? new byte[6] : "seedta"u8.ToArray(), Cp1252.Encode(text).ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void NegativeLengthsKeepTheRangeErrorAndOutput(int shape)
    {
        using var stream = Input(shape, -1, []);
        string text = "old";
        Assert.Throws<ArgumentOutOfRangeException>(() => Read(shape, ref text, stream));
        Assert.Equal(shape == 1 ? "" : "old", text);
    }

    private sealed class FailingBodyStream : MemoryStream
    {
        internal int BodyPosition;

        public override int Read(Span<byte> buffer)
        {
            if (Position < BodyPosition) return base.Read(buffer);
            if (!buffer.IsEmpty) buffer[0] = 0x80;
            throw new IOException("body failure");
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void FailingReadsPublishOnlyTheResize(int shape)
    {
        using var input = Input(shape, 6, []);
        using var stream = new FailingBodyStream { BodyPosition = (int)input.Length };
        stream.Write(input.ToArray());
        stream.Position = 0;
        string text = "seedtail";
        Assert.Throws<IOException>(() => Read(shape, ref text, stream));
        Assert.Equal(shape == 1 ? new byte[6] : "seedta"u8.ToArray(), Cp1252.Encode(text).ToArray());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void StringListsKeepNulTerminationAndObjectIds(bool extended)
    {
        var source = new StringList();
        source.AddObject("\u20AC\u00FF", Runtime.IntToObject(17));
        source.AddObject("tail\0ignored", Runtime.IntToObject(-23));
        using var stream = new MemoryStream();
        if (extended) EWriteStorageStream.StringsToStreamEx(source, stream);
        else EWriteStorageStream.StringsToStream(source, stream);
        Assert.Equal(new byte[] { 8, 0, 0, 0, 0x80, 0xFF, 13, 10, 116, 97, 105, 108 }, stream.GetBuffer()[..12]);
        stream.Position = 0;
        var result = new StringList();
        if (extended) EReadStorageStream.StringsFromStreamEx(result, stream);
        else EReadStorageStream.StringsFromStream(result, stream);
        Assert.Equal(2, result.Count);
        Assert.Equal("\u20AC\u00FF", result[0]);
        Assert.Equal("tail", result[1]);
        if (extended)
        {
            Assert.Equal(17, Runtime.ObjectToInt(result.Objects[0]));
            Assert.Equal(-23, Runtime.ObjectToInt(result.Objects[1]));
        }
        Assert.Equal(stream.Length, stream.Position);
    }

    [Fact]
    public void MemoWalkKeepsWrappingColorsAndNulTermination()
    {
        var font = new ImageLibraryFont(null!);
        var memo = new AOWMemo(null!)
        {
            FNoFrame = true,
            FWinWidth = 28,
            FWinHeight = 30,
            FAOWFont = font,
            FColorChangeChar = (byte)'#'
        };
        memo.FItems.Add("#80FF00A B\0ignored");
        memo.FItems.Add("\u20AC \u00FF\u00FF\u00FF");
        memo.SetSize();
        Assert.Equal(3, memo.FLines.Count);
        Assert.Equal("A B", memo.FLines[0]);
        Assert.Equal("\u20AC ", memo.FLines[1]);
        Assert.Equal("\u00FF\u00FF\u00FF", memo.FLines[2]);
        var color = Assert.IsType<ColorChange>(memo.FColorList[0]);
        Assert.True(color.FHasColor);
        Assert.Equal(0x80FF00, color.FColor);
    }

    [Fact]
    public void HtmlWalkKeepsTagOffsetsAndNulTermination()
    {
        var memo = new TaowHTMLMemo(null!) { FWinWidth = 100, FWinHeight = 30 };
        memo.FItems.Add("<INT><COLOR=80FF00><SPACING=3><ALIGN=RIGHT>\0<COLOR=FFFFFF>");
        memo.SetSize();
        Assert.Equal(4, memo.FChanges.Count);
        var color = Assert.IsType<TaowHTMLMemo.ChangeInfo>(memo.FChanges[1]);
        var spacing = Assert.IsType<TaowHTMLMemo.ChangeInfo>(memo.FChanges[2]);
        var alignment = Assert.IsType<TaowHTMLMemo.ChangeInfo>(memo.FChanges[3]);
        Assert.Equal(0x80FF00, color.FColor);
        Assert.Equal(3, spacing.FSpacing);
        Assert.Equal(TaowTextAlign.Right, alignment.FAlign);
        Assert.Equal(0x80FF00, alignment.FColor);
    }

    [Fact]
    public void HtmlWordWrappingKeepsItsExistingRangeError()
    {
        var memo = new TaowHTMLMemo(null!)
        {
            FWinWidth = 100,
            FWinHeight = 30,
            FAOWFont = new ImageLibraryFont(null!)
        };
        memo.FItems.Add("<INT>text");
        Assert.Throws<IndexOutOfRangeException>(() => memo.SetSize());
    }
}
