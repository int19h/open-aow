#nullable disable
using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

internal static class EffectPaths
{

    internal static void MakeLinePath(int x1, int y1, int x2, int y2, int steps, Point3DList list)
    {
        Point3D xYZ;
        if (steps < 2)
        {
            steps = 2;
        }

        for (int i = 0; i <= steps - 1; i++)
        {
            xYZ = new Point3D();
            list.Add(xYZ);
            xYZ.X = x1 + ((x2 - x1) * i / (steps - 1));
            xYZ.Y = y1 + ((y2 - y1) * i / (steps - 1));
        }
    }

    internal static void MakeSwarmPath(int step, int radius, int phase, int angle, int x, int y, SwarmDirection direction, Point3DList list)
    {
        int total;
        int b;
        float radiusLocal;
        float r;
        Point3D xYZ;
        total = 0;
        do
        {
            radiusLocal = (float)((double)(angle * Math.PI) / 180);
            b = (phase + angle) % 180;
            r = b switch
            {
                >= 0 and <= 89 => (float)((double)((double)((double)b / 90 * ((double)b / 90)) * radius) + radius),
                _ => (float)((double)((double)((double)(180 - b) / 90 * ((double)(180 - b) / 90)) * radius) + radius),
            };
            xYZ = new Point3D
            {
                X = Round((double)Math.Sin(radiusLocal) * r) + x,
                Y = Round((double)Math.Cos(radiusLocal) * r) + y
            };
            list.Add(xYZ);
            switch (direction)
            {
                case SwarmDirection.Forward:
                    angle += step;
                    if (angle >= 360)
                    {
                        angle -= 360;
                    }

                    break;
                case SwarmDirection.Backward:
                    angle -= step;
                    // { sic: tests 360 instead of 0, so the angle is never wrapped back }
                    if (angle < 360)
                    {
                        angle += 360;
                    }

                    break;
                default:
                    break;
            }

            total += step;
        }
        while (!(total >= 360));
    }

    internal static void MakeLightningPath(int x1, int y1, int x2, int y2, Point3DList list)
    {
        int hexX;
        int hexY;
        int cx;
        int cy;
        int length;
        float angle;
        float a2;
        int dist;
        int seg;
        cx = x1;
        cy = y1;
        angle = GetAngle(x1, y1, x2, y2);
        a2 = angle;
        length = Round(Math.Sqrt(((x2 - x1) * (x2 - x1)) + ((y2 - y1) * (y2 - y1))));
        dist = 0;
        seg = Random(15) + 5;
        while (dist + seg < length)
        {
            a2 = (double)a2 > angle ? (float)((double)angle - DegToRad(Random(40) + 40)) : (float)((double)angle + DegToRad(Random(40) + 40));

            double cos = Math.Cos(angle);
            double mathCos = Math.Cos(a2);
            hexX = x1 - Round((double)cos * dist) - Round((double)mathCos * seg);
            double sin = Math.Sin(angle);
            double mathSin = Math.Sin(a2);
            hexY = y1 - Round((double)sin * dist) - Round((double)mathSin * seg);
            MakeLinePath(cx, cy, hexX, hexY, seg, list);
            cx = hexX;
            cy = hexY;
            seg = Math.Abs(Round((double)Math.Cos((double)a2 - angle) * seg));
            dist += seg;
            seg = Random(15) + 5;
        }

        MakeLinePath(cx, cy, x2, y2, length - dist, list);
        int count = list.Count;
        list[count - 1].Z = 1;
    }

    internal static void MakeArcPath(int x, int y, int angle, ref float z, in float speed, ref float vz, in float gravity, Point3DList list)
    {
        float effect;
        float fy;
        float deltaX;
        float deltaY;
        Point3D xYZ;
        effect = x;
        fy = y;
        deltaX = (float)((double)Math.Cos(angle) * speed);
        deltaY = (float)((double)Math.Sin(angle) * speed);
        if ((double)gravity > 0)
        {
            while ((double)z >= 0)
            {
                xYZ = new Point3D
                {
                    X = Round(effect),
                    Y = Round(fy),
                    Z = Round(z)
                };
                list.Add(xYZ);
                effect = (float)((double)effect + deltaX);
                fy = (float)((double)fy + deltaY);
                vz = (float)((double)vz - gravity);
                z = (float)((double)z + vz);
            }
        }
    }

    internal static void MakeArcPathXY(short x1, short y1, short x2, short y2, short stepSize, short height, Point3DList list)
    {
        int i;
        int steps;
        int deltaX;
        int deltaY;
        Point3D xYZ;
        i = 0;
        deltaX = Math.Abs(x2 - x1);
        deltaY = Math.Abs(y2 - y1);
        steps = (deltaX + deltaY) / stepSize;
        if (steps < 2)
        {
            steps = 2;
        }

        while (i <= steps)
        {
            xYZ = new Point3D();
            list.Add(xYZ);
            xYZ.X = x1 + ((x2 - x1) * i / steps);
            xYZ.Y = y1 + ((y2 - y1) * i / steps);
            double pi = Math.PI;
            xYZ.Z = Round((double)Math.Sin((double)(i * pi) / steps) * height);
            i += 1;
        }
    }

    internal static float GetAngle(int x1, int y1, int x2, int y2)
    {
        float result = x1 == x2 ? (float)(Math.PI / 2) : (float)Math.Atan((double)(y2 - y1) / (x2 - x1));
        if (x2 > x1)
        {
            result = (float)((double)result + Math.PI);
        }
        else
        {
            if (y1 < y2 && x2 == x1)
            {
                result = (float)((double)result + Math.PI);
            }
        }

        return result;
    }

    internal static void CreateFire(int chance, int hexX, int hexY, GameEngine engine)
    {
        CombatFire fire;
        bool canBurn;
        CombatImageLibraryTerrain terrain;
        CityWall wall;
        if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType != 15)
        {
            if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild(2229578) == null)
            {
                if (StrategicMap.Current.Random(100) < chance)
                {
                    canBurn = false;
                    if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).GetDecorationType() is 9 or 1 or 2 or 3)
                    {
                        canBurn = true;
                    }
                    else
                    {
                        terrain = CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild<CombatImageLibraryTerrain>(2228534);
                        if (terrain != null)
                        {
                            if (terrain.GetBurnValue() > 0)
                            {
                                canBurn = true;
                            }
                        }

                        wall = CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild<CityWall>(2229508);
                        if (wall != null)
                        {
                            if (wall.WallType == 1)
                            {
                                if (wall.HitPoints > 0)
                                {
                                    canBurn = true;
                                }
                            }
                        }
                    }

                    if (canBurn)
                    {
                        fire = new CombatFire(null) { Holder = engine };
                        if (!CombatMap.Current.PlaceHex(fire, hexX, hexY, 0))
                        {
                            fire?.Dispose();
                        }
                    }
                }
            }
        }
    }

}
