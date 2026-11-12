using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Surface
{
    public int BlitClipped(Surface target, int x, int y)
        => BlitClipped(target, default, x, y);

    public int BlitAreaClipped(Surface target, in Rect area, int x, int y)
        => BlitClipped(target, area, x, y);

    public int BlitAreaColorKeyClipped(Surface target, in Rect area, int x, int y, int colorKey)
        => BlitColorKeyClipped(target, area, x, y, colorKey);
}
