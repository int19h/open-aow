using System.Runtime.InteropServices;

namespace Aow.Rtl.Tests;

public sealed class GraphicsBoundaryTests
{
    [Fact]
    public void PointerViewsKeepTheirBytesAfterBoxingAndAlignment()
    {
        var words = new[] { 0x12345678, unchecked((int)0xABCDEF01) };
        var bytes = Rtl.BytePointer(Ptr<int>.To(words, 0).Box());
        Assert.Equal(0x56, bytes[1]);
        Rtl.AlignPointer(bytes + 7, -4).Reinterpret<int>().Value = 17;
        Assert.Equal(17, words[1]);
        Assert.Equal(8, bytes.AsSpan().Length);
        Assert.Equal(0, Rtl.BytePointer(null).AsSpan().Length);
    }

    [Fact]
    public void CharacterBufferConversionStopsAtZero()
    {
        Assert.Equal("AB", Rtl.StrPasBuffer(new byte[] { 65, 66, 0, 67 }));
        Assert.Equal("AB", Rtl.StrPasBuffer(new byte[] { 65, 66 }));
        Assert.Equal(18, Marshal.SizeOf<TWaveFormatEx>());
    }

    [Fact]
    public void ShortStringImagesTruncateAndRetainTheUnusedTail()
    {
        byte[] image = [0, 0xA5, 0xA5, 0xA5];
        Rtl.WriteShortString(image, "ABCDE");
        Assert.Equal(new byte[] { 3, 65, 66, 67 }, image);
        Rtl.WriteShortString(image, "X");
        Assert.Equal(new byte[] { 1, 88, 66, 67 }, image);
        Assert.Equal("X", Rtl.ReadShortString(image));
    }

    [Fact]
    public void ThunkReferenceIdsRecoverTheOriginalObject()
    {
        var value = new TObject();
        int id = Rtl.ObjectToInt(value);
        Assert.Same(value, Rtl.ReferenceFromId<TObject>(id));
        Assert.Null(Rtl.ReferenceFromId<TObject>(0));
        Assert.Equal(-7, Rtl.ObjectToInt(Rtl.IntToObject(-7)));
    }

    private delegate void OtherNotify(TObject sender);

    [Fact]
    public void ProceduralCastsPreserveMethodIdentity()
    {
        int count = 0;
        TNotifyEvent original = _ => count++;
        var converted = Rtl.RebindDelegate<OtherNotify>(original)!;
        Assert.Same(original.Target, converted.Target);
        Assert.Equal(original.Method, converted.Method);
        converted(new TObject());
        Assert.Equal(1, count);
    }
}
