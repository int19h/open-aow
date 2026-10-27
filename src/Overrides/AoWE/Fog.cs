using Aow.Units.GFXE;
using Aow.Units.HSEngine;

namespace Aow.Units.AoWE;

public partial class TAbstractAoWHSMap
{
    // AoWHSMap.PAS:666 advances a pointer one row. GetField retains that row order.
    public void DrawFog(in TSurfaceDesc surface)
    {
        bool exploration = FExplorationEnabled || AoWE.AoWHSMap.FSeatedPlayer == -1;
        if (!FFogEnabled && !exploration) return;
        var level = MapContainer.GetMapLevel(ViewL);
        for (int x = ViewXhx; x < ViewXhx + ViewWidthHX; x++)
        {
            int xhp = 0, yhp = 0;
            HSEngine.HSEngine.HXtoHP(x, ViewYhx, ref xhp, ref yhp);
            xhp -= ViewXhp + 8;
            yhp -= ViewYhp + 4;
            for (int y = ViewYhx; y < ViewYhx + ViewHeightHX; y++, yhp += 32)
            {
                var field = (TAoWMapField)level.GetField(x, y);
                bool unexplored = exploration && (FSeatedPlayer == -1 || !field.GetExplored(FSeatedPlayer));
                if (unexplored) AoWE.AoWHSSet.FGeneralImages[3].Show(xhp, yhp, surface);
                else if (FFogEnabled && ((field.FFieldFlags >> 16) & 255) == 0)
                    AoWE.AoWHSSet.FGeneralImages[1].Show(xhp, yhp, surface);
            }
        }
    }
}
