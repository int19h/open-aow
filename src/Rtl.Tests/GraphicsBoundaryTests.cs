using System.Runtime.InteropServices;

namespace Aow.Rtl.Tests;

public sealed class GraphicsBoundaryTests
{
    [Fact]
    public void PointerViewsKeepTheirBytesAfterBoxingAndAlignment()
    {
        var words = new[] { 0x12345678, unchecked((int)0xABCDEF01) };
        var bytes = RtlUnit.BytePointer(Ptr<int>.To(words, 0).Box());
        Assert.Equal(0x56, bytes[1]);
        RtlUnit.AlignPointer(bytes + 7, -4).Reinterpret<int>().Value = 17;
        Assert.Equal(17, words[1]);
        Assert.Equal(8, bytes.AsSpan().Length);
        Assert.Equal(0, RtlUnit.BytePointer(null).AsSpan().Length);
    }

    [Fact]
    public void CharacterBufferConversionStopsAtZero()
    {
        Assert.Equal("AB", RtlUnit.StrPasBuffer(new byte[] { 65, 66, 0, 67 }));
        Assert.Equal("AB", RtlUnit.StrPasBuffer(new byte[] { 65, 66 }));
        Assert.Equal(18, Marshal.SizeOf<TWaveFormatEx>());
    }

    [Fact]
    public void ShortStringImagesTruncateAndRetainTheUnusedTail()
    {
        byte[] image = [0, 0xA5, 0xA5, 0xA5];
        RtlUnit.WriteShortString(image, "ABCDE");
        Assert.Equal(new byte[] { 3, 65, 66, 67 }, image);
        RtlUnit.WriteShortString(image, "X");
        Assert.Equal(new byte[] { 1, 88, 66, 67 }, image);
        Assert.Equal("X", RtlUnit.ReadShortString(image));
    }

    private delegate void OtherNotify(TObject sender);

    [Fact]
    public void ProceduralCastsPreserveMethodIdentity()
    {
        int count = 0;
        TNotifyEvent original = _ => count++;
        var converted = RtlUnit.RebindDelegate<OtherNotify>(original)!;
        Assert.Same(original.Target, converted.Target);
        Assert.Equal(original.Method, converted.Method);
        converted(new TObject());
        Assert.Equal(1, count);
    }
}
