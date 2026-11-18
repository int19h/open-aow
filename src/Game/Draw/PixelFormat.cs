namespace Aow.Game.Draw;

public enum PixelFormat
{
    None = 0,
    Pal8 = 8,
    Rgb555 = 1431343888,
    Argb1555 = 1431409424,
    Rgb565 = 1448121104,
    Rgb24 = -2004839656,
    Bgr24 = -2004870376,
    Rgb32 = -2004315360,
    Xrgb32 = -2004839648,
}

public static class PixelFormatExtensions
{
    public static int BitCount(this PixelFormat format) => (int)format & 255;
}
