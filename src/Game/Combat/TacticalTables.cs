#nullable disable

using Aow.Game.Draw;
using Aow.Game.World.Map;
using Aow.Game.World.Terrain;
using Aow.Rtl;
using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Combat;

internal static class TacticalTables
{
    internal static ushort GetMeleeDirectionIndex(int X, int deltaX, int deltaY)
    {
        ushort result = 1;
        switch (deltaY)
        {
            case -1:
                switch (deltaX)
                {
                    case -1:
                        result = 3;
                        break;
                    case 0:
                        result = 4;
                        break;
                    case 1:
                        result = 5;
                        break;
                }

                break;
            case 0:
                switch (deltaX)
                {
                    case -1:
                        result = X / 2 * 2 == X ? (ushort)3 : (ushort)2;

                        break;
                    case 1:
                        result = X / 2 * 2 == X ? (ushort)5 : (ushort)6;

                        break;
                }

                break;
            case 1:
                switch (deltaX)
                {
                    case -1:
                        result = 2;
                        break;
                    case 0:
                        result = 1;
                        break;
                    case 1:
                        result = 6;
                        break;
                }

                break;
        }

        return result;
    }

    internal static ReadOnlySpan<int> TacticalBattleAttack => [0, 0];

    internal static ReadOnlySpan<int> TacticalBattleDamage => [0, 0];

    internal static ReadOnlySpan<int> RangedAttackDirection => [0, 0, 0, 1, 0, 2, 3, 4, 5, 6, 0, 0, 0, 0, 0, 0, 0, 0];

    internal static ReadOnlySpan<BattleEffectType> AbilityTypes => [BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Arrow, BattleEffectType.None, BattleEffectType.Rock, BattleEffectType.Rock, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.Touch, BattleEffectType.Touch, BattleEffectType.Breath, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Rock, BattleEffectType.Rock, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Ray, BattleEffectType.None, BattleEffectType.Arrow, BattleEffectType.Arrow, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.Arrow, BattleEffectType.Comet, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Breath, BattleEffectType.Breath, BattleEffectType.Breath, BattleEffectType.Breath, BattleEffectType.Breath, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Comet, BattleEffectType.Comet, BattleEffectType.Comet, BattleEffectType.Comet, BattleEffectType.Comet, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Touch, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None];

    internal static ReadOnlySpan<BattleEffectType> SpellTypes => [BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.None, BattleEffectType.CometBall, BattleEffectType.Effect, BattleEffectType.HighPrayer, BattleEffectType.Ray, BattleEffectType.Cloud, BattleEffectType.ScreenNESW, BattleEffectType.MindDecay, BattleEffectType.Chain, BattleEffectType.Breath, BattleEffectType.None, BattleEffectType.Effect, BattleEffectType.Effect, BattleEffectType.Tremors, BattleEffectType.Effect, BattleEffectType.Breath, BattleEffectType.ScreenSwarm, BattleEffectType.ScreenNS, BattleEffectType.Rock, BattleEffectType.Geyser, BattleEffectType.AltArrow, BattleEffectType.FrostBeam, BattleEffectType.Effect, BattleEffectType.Rock, BattleEffectType.CometBall, BattleEffectType.ScreenNS, BattleEffectType.SacredWrath, BattleEffectType.Winds, BattleEffectType.ShockWave, BattleEffectType.SacFlame, BattleEffectType.Resurrect, BattleEffectType.Resurrect];

    internal static ReadOnlySpan<int> BreathHexNumber => [1, 18, 7, 8, 36, 19, 20, 59, 60, 37, 38, 39, 2, 8, 9, 10, 21, 22, 23, 39, 40, 41, 42, 43, 3, 10, 11, 12, 24, 25, 26, 43, 44, 45, 46, 47, 4, 12, 13, 14, 27, 28, 29, 47, 48, 49, 50, 51, 5, 14, 15, 16, 30, 31, 32, 51, 52, 53, 54, 55, 6, 16, 17, 18, 33, 34, 35, 55, 56, 57, 58, 59];

    internal static ReadOnlySpan<int> BreathDirection => [1, 2, 3, 4, 5, 6, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 1, 1, 2, 2, 2, 3, 3, 3, 4, 4, 4, 5, 5, 5, 6, 6, 6, 1, 1, 1, 1, 2, 2, 2, 2, 3, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 6, 6, 6, 6, 1];

    internal static ReadOnlySpan<int> WallMaxHitPoints => [7, 13];

    internal static void MakeAbilityPath(CombatSideIndex side, short ability, short spell, short x1, short y1, short x2, short y2, ref Point3DList path, ref DelphiList<RangedObstacle> obstacles)
    {
        int x = default;
        int y = default;
        switch (ability)
        {
            case 55:
                MakeRangedPath(side, 25, 0, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                break;
            case 22:
            case 24:
            case 25:
                MakeRangedPath(side, 50, 100, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                break;
            case 44:
            case 45:
            case 57:
            case 58:
            case 123:
                MakeRangedPath(side, 50, 20, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                break;
            case 30:
            case 31:
            case >= 86 and <= 90:
                path.Add(new Point3D());
                HexGeometry.HexToPixel(x2, y2, out x, out y);
                path[0].X = x + 12;
                path[0].Y = y + 12;
                obstacles.Clear();
                break;
            case 52:
                switch (spell)
                {
                    case 103:
                        MakeRangedPath(side, 25, 0, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                        break;
                    case 107:
                        MakeRangedPath(side, 25, 0, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                        obstacles.Clear();
                        break;
                    case 117:
                        MakeRangedPath(side, 50, 50, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                        break;
                    case 100:
                        MakeRangedPath(side, 50, 35, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                        break;
                    case 119:
                        MakeRangedPath(side, 50, 20, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                        break;
                    default:
                        switch (SpellTypes[spell])
                        {
                            case BattleEffectType.Breath:
                            case BattleEffectType.Effect:
                            case BattleEffectType.ScreenNS:
                            case BattleEffectType.ScreenSwarm:
                            case BattleEffectType.Geyser:
                            case BattleEffectType.Tremors:
                            case BattleEffectType.Cloud:
                            case BattleEffectType.Resurrect:
                            case BattleEffectType.FrostBeam:
                            case BattleEffectType.MindDecay:
                            case BattleEffectType.ShockWave:
                            case BattleEffectType.SacFlame:
                            case BattleEffectType.Winds:
                                path.Add(new Point3D());
                                HexGeometry.HexToPixel(x2, y2, out x, out y);
                                path[0].X = x + 12;
                                path[0].Y = y + 12;
                                obstacles.Clear();
                                break;
                            default:
                                MakeRangedPath(side, 50, 50, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                                break;
                        }

                        break;
                }

                break;
            default:
                MakeRangedPath(side, 50, 50, 50, x1, y1, x2, y2, 0, 0, true, ref path, ref obstacles);
                break;
        }
    }

    internal static void MakeRangedPath(CombatSideIndex side, short speed, short arc, short unusedHeight, short x1, short y1, short x2, short y2, short offsetX, short offsetY, bool includeUnits, ref Point3DList path, ref DelphiList<RangedObstacle> obstacles)
    {
        int i;
        int a;
        int b;
        int hexX = default;
        int hexY = default;
        int steps;
        Point3D point;
        float height;
        RangedObstacle obstacle;
        TacticalBattleUnitHexSprite unitHexSprite;
        CityWall wall;
        path.Clear();
        obstacles.Clear();
        HexGeometry.HexToPixel(x1, y1, out int x, out int y);
        HexGeometry.HexToPixel(x2, y2, out int lastX, out int lastY);
        x += 13;
        y += 13;
        lastX = lastX + 13 + offsetX;
        lastY = lastY + 13 + offsetY;
        i = 0;
        a = Math.Abs(x2 - x1);
        b = Math.Abs(y2 - y1);
        steps = (a + b) * 100 / speed;
        if (steps < 2)
        {
            steps = 2;
        }

        while (i <= steps)
        {
            point = new Point3D();
            path.Add(point);
            point.X = ((lastX - x) * i / steps) + x;
            point.Y = ((lastY - y) * i / steps) + y;
            b = steps / 2;
            a = i > b ? steps - i + 1 : i == b ? i : i + 1;

            if (arc == 0)
            {
                point.Z = 0;
            }
            else
            {
                height = a * steps * arc / 10;
                point.Z = Round(Math.Sqrt(height)) - (steps * 10 / arc);
            }

            i += 1;
        }

        lastX = 0;
        lastY = 0;
        point = path[0];
        HexGeometry.PixelToHex(point.X, point.Y, out int startX, out int startY);
        int count = path.Count;
        point = path[count - 1];
        HexGeometry.PixelToHex(point.X, point.Y, out int endX, out int endY);
        i = 0;
        int last = path.Count - 1;
        for (; i <= last; i += 1)
        {
            point = path[i];
            HexGeometry.PixelToHex(point.X, point.Y, out x, out y);
            if ((x != startX || y != startY) && (x != endX || y != endY))
            {
                steps = 0;
                if (CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].TerrainType == 15)
                {
                    steps = 3;
                }
                else
                {
                    if (CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].TerrainType == 7)
                    {
                        steps = 3;
                    }
                    else
                    {
                        a = HexGeometry.DeltaHexToRadius(x, y, startX, startY);
                        unitHexSprite = !includeUnits
                            ? null
                            : (TacticalBattleUnitHexSprite)CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].FindOwnedChild(2228484);

                        wall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].FindOwnedChild(2229508);
                        if (wall == null && side == CombatSideIndex.Attacker)
                        {
                            wall = CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].FindOwnedChild(2229510) as CityWall;
                        }

                        if (a == 1)
                        {
                            if (unitHexSprite != null && unitHexSprite.Unit.GetSide() != side)
                            {
                                steps = 1;
                            }

                            if (wall != null && wall.HitPoints > 0 && side == CombatSideIndex.Attacker)
                            {
                                steps = 4;
                            }
                        }
                        else
                        {
                            if (a > 1)
                            {
                                if (unitHexSprite != null)
                                {
                                    steps = 1;
                                }

                                if (wall == null)
                                {
                                    if (CombatMap.Current.MapContainer.MapLevels[0].Field[x, y].OverlayType == 8)
                                    {
                                        steps = 1;
                                    }
                                }
                                else
                                {
                                    if (wall.HitPoints > 0)
                                    {
                                        if (a > 2)
                                        {
                                            steps = 2;
                                        }
                                        else
                                        {
                                            wall = null;
                                            b = 1;
                                            while (wall == null && b < 7)
                                            {
                                                HexGeometry.CenterHexNumberToHex(startX, startY, b, ref hexX, ref hexY);
                                                wall = (CityWall)CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2229508) ?? CombatMap.Current.MapContainer.MapLevels[0].Field[hexX, hexY].FindOwnedChild(2229510) as CityWall;
                                                b += 1;
                                            }

                                            if (wall == null)
                                            {
                                                steps = 2;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (steps > 0)
                {
                    switch (steps)
                    {
                        case 1:
                            HexGeometry.HexToPixel(x, y, out a, out b);
                            a = point.X + 11 - a;
                            if (a > 23)
                            {
                                a = 47 - a;
                            }

                            b = point.Y + 3 - b;
                            if (b > 15)
                            {
                                b = 31 - b;
                            }

                            a = (a + b - point.Z) * 2;
                            break;
                        case 2:
                            a = 200 - (point.Z * 3);
                            break;
                        case 3:
                            a = 100;
                            break;
                        case 4:
                            a = 50;
                            break;
                    }

                    if (a > 0)
                    {
                        if (x != lastX || y != lastY)
                        {
                            obstacle = new RangedObstacle();
                            obstacle.PathIndex = i;
                            obstacles.Add(obstacle);
                            lastX = x;
                            lastY = y;
                            steps = 1;
                        }
                        else
                        {
                            obstacle = obstacles[obstacles.Count - 1];
                            if (steps == 2 && obstacle.Chance < a)
                            {
                                obstacle.Chance = a;
                            }
                        }

                        if (steps != 2 && steps != 4)
                        {
                            obstacle.Chance += a;
                        }

                        if (obstacle.Chance > 100)
                        {
                            obstacle.Chance = 100;
                        }
                    }
                }
            }
        }
    }

}
