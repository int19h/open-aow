
using Aow.Game.Draw;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class HitChance
{
    internal static bool HitRole(int attackDefense)
    {
        attackDefense = attackDefense * 10 + 50;
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
        int chance;
        chance = attackDefense * 10 + 50;
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

        if (result > 100)
        {
            result = 100;
        }

        return result;
    }

    internal static void ShowHitPercent(int X, int Y, in LockedSurface surface, int percent)
    {
        LockedSurface lockedSurface;
        lockedSurface = surface;
        GamePack.Current.CombatImages[0].Show(X + 20, Y + 54, lockedSurface);
        lockedSurface.ClipRectangle.Right = X + 20 + (percent + 3) / 4;
        if (lockedSurface.ClipRectangle.Right > surface.ClipRectangle.Right)
        {
            lockedSurface.ClipRectangle.Right = surface.ClipRectangle.Right;
        }

        GamePack.Current.CombatImages[1].Show(X + 20, Y + 54, lockedSurface);
    }

}
