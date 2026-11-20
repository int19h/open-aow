namespace Aow.Game.Draw;

public enum PixelFormat
{
    None = 0,
    Pal8 = 8,
    Rgb555 = 1_431_343_888,
    Argb1555 = 1_431_409_424,
    Rgb565 = 1_448_121_104,
    Rgb24 = -2_004_839_656,
    Bgr24 = -2_004_870_376,
    Rgb32 = -2_004_315_360,
    Xrgb32 = -2_004_839_648,
}

public static class PixelFormatExtensions
{
    public static int BitCount(this PixelFormat format) => (int)format & 0xFF;
}
