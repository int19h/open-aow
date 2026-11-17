using Aow.Game.Draw.Images;
using Aow.Game.UI.Controls;
using Aow.Game.UI.Windows;

namespace Aow.Game.Tests;

public sealed class StringBufferTests
{
    private static void Read(int shape, ref string text, Stream stream)
    {
        if (shape == 0)
        {
            GameEngine.StringFromStream(ref text, stream);
        }
        else
        {
            LibraryDrawing.ReadStringFromStream(ref text, stream);
        }
    }

    private static MemoryStream Input(int shape, int length, ReadOnlySpan<byte> bytes)
    {
        MemoryStream stream = new();
        stream.Write(Bytes.Of(ref length));
        stream.Write(bytes);
        stream.Position = 0;
        return stream;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void StringReadersKeepEveryByteAndIndependentAliases(int shape)
    {
        byte[] bytes = Enumerable.Range(0, 256).Select(i => (byte)i).ToArray();
        using MemoryStream stream = Input(shape, bytes.Length, bytes);
        string text = "old", alias = text;
        Read(shape, ref text, stream);
        Assert.Equal(bytes, Cp1252.Encode(text).ToArray());
        Assert.Equal("old", alias);
        Assert.Equal(stream.Length, stream.Position);
    }

    [Fact]
    public void ShortStringFromStreamKeepsTheResizedTail()
    {
        using MemoryStream stream = Input(0, 6, [0, 0x80, 0xFF]);
        string text = "seedtail";
        GameEngine.StringFromStream(ref text, stream);
        Assert.Equal(new byte[] { 0, 0x80, 0xFF, 100, 116, 97 }, Cp1252.Encode(text).ToArray());
    }

    [Theory]
    [InlineData(2)]
    public void ShortStringBodiesThrowAndKeepTheResize(int shape)
    {
        using MemoryStream stream = Input(shape, 6, [0, 0x80, 0xFF]);
        string text = "seedtail";
        Assert.Throws<EndOfStreamException>(() => Read(shape, ref text, stream));
        Assert.Equal("seedta"u8.ToArray(), Cp1252.Encode(text).ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void NegativeLengthsKeepTheRangeErrorAndOutput(int shape)
    {
        using MemoryStream stream = Input(shape, -1, []);
        string text = "old";
        Assert.Throws<ArgumentOutOfRangeException>(() => Read(shape, ref text, stream));
        Assert.Equal("old", text);
    }

    private sealed class FailingBodyStream : MemoryStream
    {
        internal int BodyPosition { get; init; }

        public override int Read(Span<byte> buffer)
        {
            if (Position < BodyPosition)
            {
                return base.Read(buffer);
            }

            if (!buffer.IsEmpty)
            {
                buffer[0] = 0x80;
            }

            throw new IOException("body failure");
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    public void FailingReadsPublishOnlyTheResize(int shape)
    {
        using MemoryStream input = Input(shape, 6, []);
        using FailingBodyStream stream = new() { BodyPosition = (int)input.Length };
        stream.Write(input.ToArray());
        stream.Position = 0;
        string text = "seedtail";
        Assert.Throws<IOException>(() => Read(shape, ref text, stream));
        Assert.Equal("seedta"u8.ToArray(), Cp1252.Encode(text).ToArray());
    }

    [Fact]
    public void MemoWalkKeepsWrappingColorsAndNulTermination()
    {
        ImageLibraryFont font = new(null!);
        Memo memo = new(null!)
        {
            NoFrame = true,
            Font = font,
            ColorChangeChar = (byte)'#'
        };
        memo.SetWinWidthWithoutInvalidate(28);
        memo.SetWinHeightWithoutInvalidate(30);
        memo.Items.Add("#80FF00A B\0ignored");
        memo.Items.Add("\u20AC \u00FF\u00FF\u00FF");
        memo.SetSize();
        Assert.Equal(3, memo.Lines.Count);
        Assert.Equal("A B", memo.Lines[0]);
        Assert.Equal("\u20AC ", memo.Lines[1]);
        Assert.Equal("\u00FF\u00FF\u00FF", memo.Lines[2]);
        ColorChange color = Assert.IsType<ColorChange>(memo.ColorList[0]);
        Assert.True(color.HasColor);
        Assert.Equal(0x80FF00, color.Color);
    }

    [Fact]
    public void HtmlWalkKeepsTagOffsetsAndNulTermination()
    {
        HtmlMemo memo = new(null);
        memo.SetWinWidthWithoutInvalidate(100);
        memo.SetWinHeightWithoutInvalidate(30);
        memo.Items.Add("<INT><COLOR=80FF00><SPACING=3><ALIGN=RIGHT>\0<COLOR=FFFFFF>");
        memo.SetSize();
        Assert.Equal(4, memo.Changes.Count);
        HtmlMemo.ChangeInfo color = Assert.IsType<HtmlMemo.ChangeInfo>(memo.Changes[1]);
        HtmlMemo.ChangeInfo spacing = Assert.IsType<HtmlMemo.ChangeInfo>(memo.Changes[2]);
        HtmlMemo.ChangeInfo alignment = Assert.IsType<HtmlMemo.ChangeInfo>(memo.Changes[3]);
        Assert.Equal(0x80FF00, color.Color);
        Assert.Equal(3, spacing.Spacing);
        Assert.Equal(TextAlignment.Right, alignment.Align);
        Assert.Equal(0x80FF00, alignment.Color);
    }

    [Fact]
    public void HtmlWordWrappingKeepsItsExistingRangeError()
    {
        HtmlMemo memo = new(null!)
        {
            Font = new ImageLibraryFont(null!)
        };
        memo.SetWinWidthWithoutInvalidate(100);
        memo.SetWinHeightWithoutInvalidate(30);
        memo.Items.Add("<INT>text");
        Assert.Throws<IndexOutOfRangeException>(() => memo.SetSize());
    }
}
