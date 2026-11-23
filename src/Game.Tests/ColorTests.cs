using Aow.Graphics;

namespace Aow.Game.Tests;

public sealed class ColorTests
{
    [Fact]
    public void PackedRoundTrip()
    {
        Color color = new(0x11, 0x22, 0x33);
        Assert.Equal(0x33_22_11, color.ToPacked());
        Assert.Equal(color, Color.FromPacked(0x33_22_11));
    }

    [Fact]
    public void NamedValues()
    {
        Assert.Equal(new Color(255, 255, 255), Color.White);
        Assert.Equal(new Color(0, 0, 0), Color.Black);
        Assert.Equal(0xFF_FF_FF, Color.White.ToPacked());
        Assert.Equal(0, Color.Black.ToPacked());
        Assert.Equal(Color.Black, default);
    }

    [Fact]
    public void FromPackedDropsTheHighByte() => Assert.Equal(Color.White, Color.FromPacked(unchecked((int)0xFF_FF_FF_FF)));
}
