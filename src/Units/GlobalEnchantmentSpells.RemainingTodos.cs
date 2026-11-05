using Aow.Units.Animation;
using Aow.Units.AoWE;
using Aow.Units.GFXE;
using static Aow.Units.HSEngine.HSEngine;

namespace Aow.Units.GlobalEnchantmentSpells;

public partial class TEnchantedRoads
{
    public void ShowAnimation(TAnimation anim, int xhpSurface, int yhpSurface, in TSurfaceDesc surface)
    {
        var map = AoWE.AoWE.AoWHSMap;
        var level = map.MapContainer.GetMapLevel(map.ViewL);
        int maxX = map.ViewXhx + map.ViewWidthHX - 1, maxY = map.ViewYhx + map.ViewHeightHX - 1;
        for (int x = map.ViewXhx; x <= maxX; x++)
        {
            int xhp = 0, yhp = 0;
            HXtoHP(x, map.ViewYhx, ref xhp, ref yhp);
            xhp = xhp - xhpSurface + 16;
            yhp = yhp - yhpSurface + 16;
            for (int y = map.ViewYhx; y <= maxY; y++, yhp += 32)
            {
                var field = (TAoWMapField)level.GetField(x, y);
                if (field.OverlayType == 4 && map.FSeatedPlayer != -1 && field.GetExplored(map.FSeatedPlayer))
                    FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - ((field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255)) & 15), surface);
            }
        }
        if (anim.FFrameCounter == 50) anim.Destroy();
    }
}

public partial class TFireMastery
{
    public void ShowAnimation(TAnimation anim, int xhpSurface, int yhpSurface, in TSurfaceDesc surface)
    {
        int chance = AoWE.AoWE.AoWEngine.FAnimationDetail switch { 0 => 2, 2 => 5, _ => 3 };
        var map = AoWE.AoWE.AoWHSMap;
        var level = map.MapContainer.GetMapLevel(map.ViewL);
        int maxX = map.ViewXhx + map.ViewWidthHX - 1, maxY = map.ViewYhx + map.ViewHeightHX - 1;
        for (int x = map.ViewXhx; x <= maxX; x++)
        {
            int xhp = 0, yhp = 0;
            HXtoHP(x, map.ViewYhx, ref xhp, ref yhp);
            xhp = xhp - xhpSurface + 16;
            yhp = yhp - yhpSurface + 16;
            for (int y = map.ViewYhx; y <= maxY; y++, yhp += 32)
            {
                var field = (TAoWMapField)level.GetField(x, y);
                if (field.TerrainType is 7 or 8 or 15 || map.FSeatedPlayer == -1 || map.FExplorationEnabled && !field.GetExplored(map.FSeatedPlayer)) continue;
                Rtl.Rtl.RandSeed = field.Xhx + field.Yhx * 8 + ((field.FFieldFlags >> 16) & 255) * 65536;
                if (Rtl.Rtl.Random(100) >= chance) continue;
                int delay = Rtl.Rtl.Random(40);
                FCastImages.Show(0, xhp, yhp, anim.FFrameCounter - delay, surface);
                if (anim.FFrameCounter == delay && AoWE.AoWE.AoWEngine.FAnimationDetail != 0)
                    Sound.Sound.PlaySample(AoWE.AoWE.ExplosionSFX[x % 2]);
            }
        }
        if (anim.FFrameCounter == 90) anim.Destroy();
    }
}
