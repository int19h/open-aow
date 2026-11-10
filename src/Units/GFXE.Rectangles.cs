using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class Surface
{
    public int BltClipped(Surface target, int x, int y)
        => BltClipped(target, default, x, y);

    public int BltAreaClipped(Surface target, in Rect area, int x, int y)
        => BltClipped(target, area, x, y);

    public int BltAreaColorKeyClipped(Surface target, in Rect area, int x, int y, int colorKey)
        => BltColorKeyClipped(target, area, x, y, colorKey);
}
