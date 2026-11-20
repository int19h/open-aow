
using Aow.Graphics;

namespace Aow.Game.Draw;

internal static class BlitRects
{

    internal static void BlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.BlitRectangle(source, d: destination, w: width, h: height);

    internal static void BlitColorKey16(SurfaceView source, SurfaceView destination, int width, int height, int colorKey) => Blit.BlitColorKey16(source, d: destination, w: width, h: height, key: colorKey);

}
