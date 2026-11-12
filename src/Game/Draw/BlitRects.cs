
using Aow.Graphics;

namespace Aow.Game.Draw;

internal static class BlitRects
{

    internal static void BlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.BlitRectangle(source, destination, width, height);

    internal static void BlitColorKey16(SurfaceView source, SurfaceView destination, int width, int height, int colorKey) => Blit.BlitColorKey16(source, destination, width, height, colorKey);

}
