
namespace Aow.Rtl.Tests;

public sealed class GraphicsBoundaryTests
{
    [Fact]
    public void CharacterBufferConversionStopsAtZero()
    {
        Assert.Equal("AB", PCharStrings.StrPasBuffer([65, 66, 0, 67]));
        Assert.Equal("AB", PCharStrings.StrPasBuffer([65, 66]));
    }

    [Fact]
    public void ShortStringImagesTruncateAndRetainTheUnusedTail()
    {
        byte[] image = [0, 0xA5, 0xA5, 0xA5];
        string value = "ABCDE";
        int length = Math.Min(value.Length, ((Span<byte>)image).Length - 1);
        ((Span<byte>)image)[0] = (byte)length;
        Cp1252.Encode(value).AsSpan()[..length].CopyTo(((Span<byte>)image)[1..]);
        Assert.Equal(new byte[] { 3, 65, 66, 67 }, image);
        string value2 = "X";
        int length2 = Math.Min(value2.Length, ((Span<byte>)image).Length - 1);
        ((Span<byte>)image)[0] = (byte)length2;
        Cp1252.Encode(value2).AsSpan()[..length2].CopyTo(((Span<byte>)image)[1..]);
        Assert.Equal(new byte[] { 1, 88, 66, 67 }, image);
        Assert.Equal("X", Cp1252.Decode(((ReadOnlySpan<byte>)image).Slice(1, length: ((ReadOnlySpan<byte>)image)[0]).ToArray()));
    }

    [Fact]
    public void ProceduralCastsPreserveMethodIdentity()
    {
        int count = 0;
        NotifyEvent<object> original = _ => count++;
        Action<object> converted = DelegateRoutines.RebindDelegate<Action<object>>(original)!;
        Assert.Same(original.Target, converted.Target);
        Assert.Equal(original.Method, converted.Method);
        converted(new object());
        Assert.Equal(1, count);
    }
}
