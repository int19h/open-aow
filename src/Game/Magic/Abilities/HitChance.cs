#nullable disable warnings // Enabled before the stage 1 annotations of issue 315.

using Aow.Game.Draw;

using Aow.Game.World.Map;
using Aow.Rtl;
using static Aow.Rtl.DelphiRandom;

namespace Aow.Game.Magic.Abilities;

internal static class HitChance
{
    internal static bool HitRole(int attackDefense)
    {
        attackDefense = (attackDefense * 10) + 50;
        if (attackDefense < 10)
        {
            attackDefense = 10;
        }

        if (attackDefense > 90)
        {
            attackDefense = 90;
        }

        return Random(100) < attackDefense;
    }

    internal static float HitRoleProbability(int attackDefense)
    {
        int chance = (attackDefense * 10) + 50;
        if (chance < 10)
        {
            chance = 10;
        }

        if (chance > 90)
        {
            chance = 90;
        }

        return (float)((double)chance / 100);
    }

    internal static int HitPointsToHitPercent(sbyte hitPoints, sbyte hits)
    {
        int result = hitPoints * 100 / hits;
        if (result == 0 && hitPoints != 0)
        {
            result = 1;
        }

        return result > 100 ? 100 : result;
    }

    internal static void ShowHitPercent(int x, int y, in LockedSurface surface, int percent)
    {
        LockedSurface lockedSurface;
        lockedSurface = surface;
        GamePack.Current.CombatImages[0].Show(x + 20, y + 54, lockedSurface);
        Rect clipRectangle = lockedSurface.ClipRectangle;
        clipRectangle.Right = x + 20 + ((percent + 3) / 4);
        lockedSurface.ClipRectangle = clipRectangle;
        if (lockedSurface.ClipRectangle.Right > surface.ClipRectangle.Right)
        {
            Rect clipRectangle2 = lockedSurface.ClipRectangle;
            clipRectangle2.Right = surface.ClipRectangle.Right;
            lockedSurface.ClipRectangle = clipRectangle2;
        }

        GamePack.Current.CombatImages[1].Show(x + 20, y + 54, lockedSurface);
    }
}
