using System.Runtime.CompilerServices;
using Aow.Game.Combat.Actions;
using Aow.Game.Commands;
using Aow.Game.Draw;
using Aow.Game.Draw.Images;
using Aow.Game.Engine;
using Aow.Game.Engine.Buffers;
using Aow.Game.Engine.Nodes;
using Aow.Game.Engine.Storage;
using Aow.Game.Magic.Abilities;
using Aow.Game.Magic.Spells;
using Aow.Game.Net;
using Aow.Game.Saves;
using Aow.Game.Session;
using Aow.Game.Units;
using Aow.Game.World;
using Aow.Game.World.Hexes;
using Aow.Game.World.Map;
using Aow.Game.World.Movement;
using Aow.Game.World.Structures;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

internal static class TacticalTables
{
    internal static ushort GetMeleeDirIndex(int X, int DX, int DY)
    {
        ushort Result = 1;
        switch (DY)
        {
            case -1:
                switch (DX)
                {
                    case -1:
                        Result = 3;
                        break;
                    case 0:
                        Result = 4;
                        break;
                    case 1:
                        Result = 5;
                        break;
                }

                break;
            case 0:
                switch (DX)
                {
                    case -1:
                        if (X / 2 * 2 == X)
                        {
                            Result = 3;
                        }
                        else
                        {
                            Result = 2;
                        }

                        break;
                    case 1:
                        if (X / 2 * 2 == X)
                        {
                            Result = 5;
                        }
                        else
                        {
                            Result = 6;
                        }

                        break;
                }

                break;
            case 1:
                switch (DX)
                {
                    case -1:
                        Result = 2;
                        break;
                    case 0:
                        Result = 1;
                        break;
                    case 1:
                        Result = 6;
                        break;
                }

                break;
        }

        return Result;
    }

    internal static FixedArray_int_2 TCAttack;

    internal static FixedArray_int_2 TCDamage;

    internal static FixedArray_int_18 raDir = FixedArray_int_18.From([0, 0, 0, 1, 0, 2, 3, 4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0]);

    internal static FixedArray_Aow_Units_AoWTC_TfxType_170 AbilTypes = FixedArray_Aow_Units_AoWTC_TfxType_170.From([TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxArrow, TfxType.fxNone, TfxType.fxRock, TfxType.fxRock, TfxType.fxNone, TfxType.fxTouch, TfxType.fxTouch, TfxType.fxTouch, TfxType.fxBreath, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxRock, TfxType.fxRock, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxRay, TfxType.fxNone, TfxType.fxArrow, TfxType.fxArrow, TfxType.fxNone, TfxType.fxTouch, TfxType.fxArrow, TfxType.fxComet, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxBreath, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxComet, TfxType.fxComet, TfxType.fxComet, TfxType.fxComet, TfxType.fxComet, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxTouch, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone]);

    internal static FixedArray_Aow_Units_AoWTC_TfxType_131 SpellTypes = FixedArray_Aow_Units_AoWTC_TfxType_131.From([TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxEffect, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxNone, TfxType.fxCometBall, TfxType.fxEffect, TfxType.fxHighPrayer, TfxType.fxRay, TfxType.fxCloud, TfxType.fxScreenNESW, TfxType.fxMindDecay, TfxType.fxChain, TfxType.fxBreath, TfxType.fxNone, TfxType.fxEffect, TfxType.fxEffect, TfxType.fxTremors, TfxType.fxEffect, TfxType.fxBreath, TfxType.fxScreenSwarm, TfxType.fxScreenNS, TfxType.fxRock, TfxType.fxGeyser, TfxType.fxAltArrow, TfxType.fxFrostBeam, TfxType.fxEffect, TfxType.fxRock, TfxType.fxCometBall, TfxType.fxScreenNS, TfxType.fxSacredWrath, TfxType.fxWinds, TfxType.fxShockWave, TfxType.fxSacFlame, TfxType.fxResurrect, TfxType.fxResurrect]);

    internal static FixedArray_int_72 BreathHN = FixedArray_int_72.From([1, 18, 7, 8, 36, 19, 20, 59, 60, 37, 38, 39, 2, 8, 9, 10, 21, 22, 23, 39, 40, 41, 42, 43, 3, 10, 11, 12, 24, 25, 26, 43, 44, 45, 46, 47, 4, 12, 13, 14, 27, 28, 29, 47, 48, 49, 50, 51, 5, 14, 15, 16, 30, 31, 32, 51, 52, 53, 54, 55, 6, 16, 17, 18, 33, 34, 35, 55, 56, 57, 58, 59]);

    internal static FixedArray_int_60 BreathDirection = FixedArray_int_60.From([1, 2, 3, 4, 5, 6, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 1]);

    internal static FixedArray_int_2 WallMaxHP = FixedArray_int_2.From([7, 13]);

    internal static void MakeAbilPath(TCombatSideIndex Side, short Ability, short Spell, short X1, short Y1, short X2, short Y2, ref TXYZList Path, ref TList Obstacles)
    {
        int X = default;
        int Y = default;
        switch (Ability)
        {
            case 55:
                Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 25, 0, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
            case 22:
            case 24:
            case 25:
                Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 50, 100, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
            case 44:
            case 45:
            case 57:
            case 58:
            case 123:
                Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 50, 20, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
            case 30:
            case 31:
            case >= 86 and <= 90:
                var xyz = new TXYZ();
                Path.Add(xyz);
                Aow.Game.World.Map.HexGeometry.HXtoHP(X2, Y2, ref X, ref Y);
                Path[0].X = X + 12;
                Path[0].Y = Y + 12;
                Obstacles.Clear();
                break;
            case 52:
                switch (Spell)
                {
                    case 103:
                        Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 25, 0, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    case 107:
                        Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 25, 0, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        Obstacles.Clear();
                        break;
                    case 117:
                        Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 50, 50, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    case 100:
                        Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 50, 35, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    case 119:
                        Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 50, 20, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                        break;
                    default:
                        switch (Aow.Game.Combat.TacticalTables.SpellTypes[Spell])
                        {
                            case TfxType.fxBreath:
                            case TfxType.fxEffect:
                            case TfxType.fxScreenNS:
                            case TfxType.fxScreenSwarm:
                            case TfxType.fxGeyser:
                            case TfxType.fxTremors:
                            case TfxType.fxCloud:
                            case TfxType.fxResurrect:
                            case TfxType.fxFrostBeam:
                            case TfxType.fxMindDecay:
                            case TfxType.fxShockWave:
                            case TfxType.fxSacFlame:
                            case TfxType.fxWinds:
                                var xyz2 = new TXYZ();
                                Path.Add(xyz2);
                                Aow.Game.World.Map.HexGeometry.HXtoHP(X2, Y2, ref X, ref Y);
                                Path[0].X = X + 12;
                                Path[0].Y = Y + 12;
                                Obstacles.Clear();
                                break;
                            default:
                                Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 50, 50, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                                break;
                        }

                        break;
                }

                break;
            default:
                Aow.Game.Combat.TacticalTables.MakeRangedPath(Side, 50, 50, 50, X1, Y1, X2, Y2, 0, 0, true, ref Path, ref Obstacles);
                break;
        }
    }

    internal static void MakeRangedPath(TCombatSideIndex Side, short Speed, short Arc, short UnusedHeight, short X1, short Y1, short X2, short Y2, short OffsetX, short OffsetY, bool IncludeUnits, ref TXYZList Path, ref TList Obstacles)
    {
        int I;
        int A;
        int B;
        int NX = default;
        int NY = default;
        int Steps;
        int X = default;
        int Y = default;
        int LastX = default;
        int LastY = default;
        int StartX = default;
        int StartY = default;
        int EndX = default;
        int EndY = default;
        TXYZ Point;
        float Height;
        TROb Obstacle;
        TTacticalCombatUnitHS UnitHS;
        TCityWall Wall;
        Path.Clear();
        Obstacles.Clear();
        Aow.Game.World.Map.HexGeometry.HXtoHP(X1, Y1, ref X, ref Y);
        Aow.Game.World.Map.HexGeometry.HXtoHP(X2, Y2, ref LastX, ref LastY);
        X += 13;
        Y += 13;
        LastX = LastX + 13 + OffsetX;
        LastY = LastY + 13 + OffsetY;
        I = 0;
        A = Math.Abs(X2 - X1);
        B = Math.Abs(Y2 - Y1);
        Steps = (A + B) * 100 / Speed;
        if (Steps < 2)
        {
            Steps = 2;
        }

        while (I <= Steps)
        {
            Point = new TXYZ();
            Path.Add(Point);
            Point.X = (LastX - X) * I / Steps + X;
            Point.Y = (LastY - Y) * I / Steps + Y;
            B = Steps / 2;
            if (I > B)
            {
                A = Steps - I + 1;
            }
            else
            {
                if (I == B)
                {
                    A = I;
                }
                else
                {
                    A = I + 1;
                }
            }

            if (Arc == 0)
            {
                Point.Z = 0;
            }
            else
            {
                Height = A * Steps * Arc / 10;
                Point.Z = Round(Math.Sqrt(Height)) - Steps * 10 / Arc;
            }

            I += 1;
        }

        LastX = 0;
        LastY = 0;
        Point = Path[0];
        Aow.Game.World.Map.HexGeometry.HPtoHX(Point.X, Point.Y, ref StartX, ref StartY);
        var count = Path.Count;
        Point = Path[count - 1];
        Aow.Game.World.Map.HexGeometry.HPtoHX(Point.X, Point.Y, ref EndX, ref EndY);
        I = 0;
        int last = Path.Count - 1;
        for (; I <= last; I += 1)
        {
            Point = Path[I];
            Aow.Game.World.Map.HexGeometry.HPtoHX(Point.X, Point.Y, ref X, ref Y);
            if ((X != StartX || Y != StartY) && (X != EndX || Y != EndY))
            {
                Steps = 0;
                if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].TerrainType == 15)
                {
                    Steps = 3;
                }
                else
                {
                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].TerrainType == 7)
                    {
                        Steps = 3;
                    }
                    else
                    {
                        A = Aow.Game.World.Map.HexGeometry.dHXtoRad(X, Y, StartX, StartY);
                        if (!IncludeUnits)
                        {
                            UnitHS = null;
                        }
                        else
                        {
                            UnitHS = (TTacticalCombatUnitHS)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2228484);
                        }

                        Wall = (TCityWall)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229508);
                        if (Wall == null && Side == TCombatSideIndex.csAttacker)
                        {
                            Wall = (TCityWall)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].FindOwnedChild(2229510);
                        }

                        if (A == 1)
                        {
                            if (UnitHS != null && UnitHS.FUnit.GetSide() != Side)
                            {
                                Steps = 1;
                            }

                            if (Wall != null && Wall.FHitPoints > 0 && Side == TCombatSideIndex.csAttacker)
                            {
                                Steps = 4;
                            }
                        }
                        else
                        {
                            if (A > 1)
                            {
                                if (UnitHS != null)
                                {
                                    Steps = 1;
                                }

                                if (Wall == null)
                                {
                                    if (Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[X, Y].OverlayType == 8)
                                    {
                                        Steps = 1;
                                    }
                                }
                                else
                                {
                                    if (Wall.FHitPoints > 0)
                                    {
                                        if (A > 2)
                                        {
                                            Steps = 2;
                                        }
                                        else
                                        {
                                            Wall = null;
                                            B = 1;
                                            while (Wall == null && B < 7)
                                            {
                                                Aow.Game.World.Map.HexGeometry.CenterHNtoHX(StartX, StartY, B, ref NX, ref NY);
                                                Wall = (TCityWall)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229508);
                                                if (Wall == null)
                                                {
                                                    Wall = (TCityWall)Aow.Game.Combat.TAoWCombatMap.AoWCombatMap.MapContainer.MapLevels[0].Field[NX, NY].FindOwnedChild(2229510);
                                                }

                                                B += 1;
                                            }

                                            if (Wall == null)
                                            {
                                                Steps = 2;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (Steps > 0)
                {
                    switch (Steps)
                    {
                        case 1:
                            Aow.Game.World.Map.HexGeometry.HXtoHP(X, Y, ref A, ref B);
                            A = Point.X + 11 - A;
                            if (A > 23)
                            {
                                A = 47 - A;
                            }

                            B = Point.Y + 3 - B;
                            if (B > 15)
                            {
                                B = 31 - B;
                            }

                            A = (A + B - Point.Z) * 2;
                            break;
                        case 2:
                            A = 200 - Point.Z * 3;
                            break;
                        case 3:
                            A = 100;
                            break;
                        case 4:
                            A = 50;
                            break;
                    }

                    if (A > 0)
                    {
                        if (X != LastX || Y != LastY)
                        {
                            Obstacle = new TROb();
                            Obstacle.PathIndex = I;
                            Obstacles.Add(Obstacle);
                            LastX = X;
                            LastY = Y;
                            Steps = 1;
                        }
                        else
                        {
                            Obstacle = (TROb)Obstacles[Obstacles.Count - 1];
                            if (Steps == 2 && Obstacle.Chance < A)
                            {
                                Obstacle.Chance = A;
                            }
                        }

                        if (Steps != 2 && Steps != 4)
                        {
                            Obstacle.Chance += A;
                        }

                        if (Obstacle.Chance > 100)
                        {
                            Obstacle.Chance = 100;
                        }
                    }
                }
            }
        }
    }

}
