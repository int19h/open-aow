
using Aow.Game.Draw;

using Aow.Game.World.Map;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Magic.Abilities;

internal static class HitChance
{
    internal static bool HitRole(int AttackDefense)
    {
        AttackDefense = AttackDefense * 10 + 50;
        if (AttackDefense < 10)
        {
            AttackDefense = 10;
        }

        if (AttackDefense > 90)
        {
            AttackDefense = 90;
        }

        return Random(100) < AttackDefense;
    }

    internal static float HitRoleProbability(int AttackDefense)
    {
        int Chance;
        Chance = AttackDefense * 10 + 50;
        if (Chance < 10)
        {
            Chance = 10;
        }

        if (Chance > 90)
        {
            Chance = 90;
        }

        return (float)((double)Chance / 100);
    }

    internal static int HitPointsToHitPcnt(sbyte HitPoints, sbyte Hits)
    {
        int Result = HitPoints * 100 / Hits;
        if (Result == 0 && HitPoints != 0)
        {
            Result = 1;
        }

        if (Result > 100)
        {
            Result = 100;
        }

        return Result;
    }

    internal static void ShowHitPcnt(int X, int Y, in SurfaceDesc Surface, int Percent)
    {
        SurfaceDesc SD;
        SD = Surface;
        AoWHSSet.Current.FCombatImages[0].Show(X + 20, Y + 54, SD);
        SD.ClipRect.Right = X + 20 + (Percent + 3) / 4;
        if (SD.ClipRect.Right > Surface.ClipRect.Right)
        {
            SD.ClipRect.Right = Surface.ClipRect.Right;
        }

        AoWHSSet.Current.FCombatImages[1].Show(X + 20, Y + 54, SD);
    }

}
