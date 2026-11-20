
using Aow.Graphics;

namespace Aow.Game.Draw;

internal static class BlitRects
{

    internal static void BlitRectangle(SurfaceView source, SurfaceView destination, int width, int height) => Blit.BlitRectangle(source, d: destination, w: width, h: height);

}
