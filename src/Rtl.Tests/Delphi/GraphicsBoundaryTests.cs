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
        ShortStrings.WriteShortString(image, value: "ABCDE");
        Assert.Equal(new byte[] { 3, 65, 66, 67 }, image);
        ShortStrings.WriteShortString(image, value: "X");
        Assert.Equal(new byte[] { 1, 88, 66, 67 }, image);
        Assert.Equal("X", ShortStrings.ReadShortString(image));
    }

    private delegate void OtherNotify(object sender);

    [Fact]
    public void ProceduralCastsPreserveMethodIdentity()
    {
        int count = 0;
        NotifyEvent<object> original = _ => count++;
        OtherNotify converted = DelegateRoutines.RebindDelegate<OtherNotify>(original)!;
        Assert.Same(original.Target, converted.Target);
        Assert.Equal(original.Method, converted.Method);
        converted(new object());
        Assert.Equal(1, count);
    }
}
