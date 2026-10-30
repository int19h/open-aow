using Aow.Units.AoWHTMLMemo;
using Aow.Units.AoWMemo;
using Aow.Units.ImageLib;
using EngineText = Aow.Units.Engine.Engine;
using Images = Aow.Units.ImageLib.ImageLib;

namespace Aow.Units.Tests;

public sealed class StringBufferTests
{
    private static void Read(int shape, ref AnsiString text, TStream stream)
    {
        if (shape == 0) EngineText.StringFromStream(ref text, stream);
        else if (shape == 1) EngineText.LoadStringFromStream(ref text, stream);
        else Images.ReadStringFromStream(ref text, stream);
    }

    private static TMemoryStream Input(int shape, int length, ReadOnlySpan<byte> bytes)
    {
        var stream = new TMemoryStream();
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
        AnsiString text = "old", alias = text;
        Read(shape, ref text, stream);
        Assert.Equal(bytes, Runtime.StringBytes(text).ToArray());
        Assert.Equal((AnsiString)"old", alias);
        Assert.Equal(stream.Size, stream.Position);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void ShortReadsKeepTheResizedTail(int shape)
    {
        using var stream = Input(shape, 6, [0, 0x80, 0xFF]);
        AnsiString text = "seedtail";
        Read(shape, ref text, stream);
        Assert.Equal(shape == 1 ? new byte[] { 0, 0x80, 0xFF, 0, 0, 0 }
            : new byte[] { 0, 0x80, 0xFF, 100, 116, 97 }, Runtime.StringBytes(text).ToArray());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void NegativeLengthsKeepTheRangeErrorAndOutput(int shape)
    {
        using var stream = Input(shape, -1, []);
        AnsiString text = "old";
        Assert.Throws<ERangeError>(() => Read(shape, ref text, stream));
        Assert.Equal(shape == 1 ? (AnsiString)"" : (AnsiString)"old", text);
    }

    private sealed class FailingBodyStream : TMemoryStream
    {
        internal int BodyPosition;
        public override int Read(Span<byte> buffer)
        {
            if (Position < BodyPosition) return base.Read(buffer);
            if (!buffer.IsEmpty) buffer[0] = 0x80;
            throw new EReadError("body failure");
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    public void FailingReadsPublishOnlyTheResize(int shape)
    {
        using var input = Input(shape, 6, []);
        using var stream = new FailingBodyStream { BodyPosition = input.Size };
        stream.Write(input.Memory.AsSpan(0, input.Size));
        stream.Position = 0;
        AnsiString text = "seedtail";
        Assert.Throws<EReadError>(() => Read(shape, ref text, stream));
        Assert.Equal(shape == 1 ? new byte[6] : "seedta"u8.ToArray(), Runtime.StringBytes(text).ToArray());
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void StringListsKeepNulTerminationAndObjectIds(bool extended)
    {
        var source = new TStringList();
        source.AddObject(AnsiString.FromBytes(0x80, 0xFF), Runtime.IntToObject(17));
        source.AddObject((AnsiString)"tail\0ignored", Runtime.IntToObject(-23));
        using var stream = new TMemoryStream();
        if (extended) EngineText.StringsToStreamEx(source, stream);
        else EngineText.StringsToStream(source, stream);
        Assert.Equal(new byte[] { 8, 0, 0, 0, 0x80, 0xFF, 13, 10, 116, 97, 105, 108 }, stream.Memory[..12]);
        stream.Position = 0;
        var result = new TStringList();
        if (extended) EngineText.StringsFromStreamEx(result, stream);
        else EngineText.StringsFromStream(result, stream);
        Assert.Equal(2, result.Count);
        Assert.Equal(AnsiString.FromBytes(0x80, 0xFF), result[0]);
        Assert.Equal((AnsiString)"tail", result[1]);
        if (extended)
        {
            Assert.Equal(17, Runtime.ObjectToInt(result.Objects[0]));
            Assert.Equal(-23, Runtime.ObjectToInt(result.Objects[1]));
        }
        Assert.Equal(stream.Size, stream.Position);
    }

    [Fact]
    public void MemoWalkKeepsWrappingColorsAndNulTermination()
    {
        var font = new TImageLibraryFont(null!);
        var memo = new TAOWMemo(null!)
        {
            FNoFrame = true, FWinWidth = 28, FWinHeight = 30,
            FAOWFont = font, FColorChangeChar = (byte)'#'
        };
        memo.FItems.Add("#80FF00A B\0ignored");
        memo.FItems.Add(AnsiString.FromBytes(0x80, 32, 0xFF, 0xFF, 0xFF));
        memo.SetSize();
        Assert.Equal(3, memo.FLines.Count);
        Assert.Equal((AnsiString)"A B", memo.FLines[0]);
        Assert.Equal(AnsiString.FromBytes(0x80, 32), memo.FLines[1]);
        Assert.Equal(AnsiString.FromBytes(0xFF, 0xFF, 0xFF), memo.FLines[2]);
        var color = Assert.IsType<TColorChange>(memo.FColorList[0]);
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
        var color = Assert.IsType<TChangeInfo>(memo.FChanges[1]);
        var spacing = Assert.IsType<TChangeInfo>(memo.FChanges[2]);
        var alignment = Assert.IsType<TChangeInfo>(memo.FChanges[3]);
        Assert.Equal(0x80FF00, color.FColor);
        Assert.Equal(3, spacing.FSpacing);
        Assert.Equal(Aow.Units.AoWComp.TaowTextAlign.taRight, alignment.FAlign);
        Assert.Equal(0x80FF00, alignment.FColor);
    }

    [Fact]
    public void HtmlWordWrappingKeepsItsExistingRangeError()
    {
        var memo = new TaowHTMLMemo(null!)
        {
            FWinWidth = 100, FWinHeight = 30, FAOWFont = new TImageLibraryFont(null!)
        };
        memo.FItems.Add("<INT>text");
        Assert.Throws<ERangeError>(() => memo.SetSize());
    }
}
