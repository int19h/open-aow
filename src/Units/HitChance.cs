using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Aow.Formats.Tree;
using Aow.Game.Ai;
using Aow.Game.Ai.Groups;
using Aow.Game.Audio;
using Aow.Game.Combat;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Animation;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Collections;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Events;
using Aow.Game.Magic;
using Aow.Game.Magic.Spells;
using Aow.Game.Magic.Spells.Unit;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Ui.Controls;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using Aow.Rtl.Localize;
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
        Aow.Game.Engine.AoWHSSet.Current.FCombatImages[0].Show(X + 20, Y + 54, SD);
        SD.ClipRect.Right = X + 20 + (Percent + 3) / 4;
        if (SD.ClipRect.Right > Surface.ClipRect.Right)
        {
            SD.ClipRect.Right = Surface.ClipRect.Right;
        }

        Aow.Game.Engine.AoWHSSet.Current.FCombatImages[1].Show(X + 20, Y + 54, SD);
    }

}
