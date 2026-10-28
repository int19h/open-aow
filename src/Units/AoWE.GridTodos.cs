using Aow.Units.GFXE;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.AoWE;

public partial class TAbstractAoWHSMap
{
    public void DrawGrid(in TSurfaceDesc surface)
    {
        var level = MapContainer.GetMapLevel(ViewL);
        int maxX = ViewXhx + ViewWidthHX - 1, maxY = ViewYhx + ViewHeightHX - 1;
        for (int x = ViewXhx; x <= maxX; x++)
        {
            int xhp = 0, yhp = 0;
            HXtoHP(x, ViewYhx, ref xhp, ref yhp);
            xhp = xhp - ViewXhp - 8;
            yhp = yhp - ViewYhp - 4 + 4;
            for (int y = ViewYhx; y <= maxY; y++, yhp += 32)
                if (((TAoWMapField)level.GetField(x, y)).TerrainType != 15)
                    AoWE.AoWHSSet.FFlagControl.FFlags[7].Show(xhp, yhp, surface);
        }
    }
}
