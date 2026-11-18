using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Surface
{
    public GraphicsResult BlitClipped(Surface target, int x, int y)
        => BlitClipped(target, default, x, y);

    public GraphicsResult BlitAreaClipped(Surface target, in Rect area, int x, int y)
        => BlitClipped(target, area, x, y);

    public GraphicsResult BlitAreaColorKeyClipped(Surface target, in Rect area, int x, int y, int colorKey)
        => BlitColorKeyClipped(target, area, x, y, colorKey);
}
