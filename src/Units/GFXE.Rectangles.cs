using Aow.Rtl;

namespace Aow.Game.Draw;

public partial class TSurface
{
    public int BltClipped(TSurface target, int x, int y)
        => BltClipped(target, default, x, y);

    public int BltAreaClipped(TSurface target, in TRect area, int x, int y)
        => BltClipped(target, area, x, y);

    public int BltAreaColorKeyClipped(TSurface target, in TRect area, int x, int y, int colorKey)
        => BltColorKeyClipped(target, area, x, y, colorKey);
}
