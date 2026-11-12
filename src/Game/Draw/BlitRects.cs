
using Aow.Graphics;

namespace Aow.Game.Draw;

internal static class BlitRects
{
    internal static void MMXBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.MMXBltRect(Src, Dst, Width, Height);

    internal static void FPBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.FPBltRect(Src, Dst, Width, Height);

    internal static void BlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.BlitRectangle(source, destination, width, height);

    internal static void BlitColorKey16(SurfaceView source, SurfaceView destination, int width, int height, int colorKey) => Blit.BlitColorKey16(source, destination, width, height, colorKey);

}
