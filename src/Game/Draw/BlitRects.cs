
using Aow.Graphics;

namespace Aow.Game.Draw;

internal static class BlitRects
{
    internal static void MMXBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.MMXBltRect(Src, Dst, Width, Height);

    internal static void FPBltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.FPBltRect(Src, Dst, Width, Height);

    internal static void BltRect(SurfaceView Src, SurfaceView Dst, int Width, int Height) => Blit.BltRect(Src, Dst, Width, Height);

    internal static void BltColorKey16(SurfaceView Src, SurfaceView Dst, int Width, int Height, int ColorKey) => Blit.BltColorKey16(Src, Dst, Width, Height, ColorKey);

}
