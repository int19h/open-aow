using Aow.Game.Combat;

using Aow.Game.World.Map;

using Aow.Game.World.Terrain;

using static Aow.Rtl.RtlUnit;

namespace Aow.Game.Draw;

internal static class EffectPaths
{

    internal static void MakeLinePath(int x1, int y1, int x2, int y2, int steps, ref Point3DList list)
    {
        Point3D xYZ;
        if (steps < 2)
        {
            steps = 2;
        }

        for (int I = 0; I <= steps - 1; I++)
        {
            xYZ = new Point3D();
            list.Add(xYZ);
            xYZ.X = x1 + (x2 - x1) * I / (steps - 1);
            xYZ.Y = y1 + (y2 - y1) * I / (steps - 1);
        }
    }

    internal static void MakeSwarmPath(int step, int radius, int phase, int angle, int X, int Y, SwarmDirection direction, ref Point3DList list)
    {
        int total;
        int B;
        float radiusLocal;
        float R;
        Point3D xYZ;
        total = 0;
        do
        {
            radiusLocal = (float)((double)((double)angle * Math.PI) / 180);
            B = (phase + angle) % 180;
            switch (B)
            {
                case >= 0 and <= 89:
                    R = (float)((double)((double)((double)B / 90 * ((double)B / 90)) * radius) + radius);
                    break;
                default:
                    R = (float)((double)((double)((double)(180 - B) / 90 * ((double)(180 - B) / 90)) * radius) + radius);
                    break;
            }

            xYZ = new Point3D();
            xYZ.X = Round((double)Math.Sin(radiusLocal) * R) + X;
            xYZ.Y = Round((double)Math.Cos(radiusLocal) * R) + Y;
            list.Add(xYZ);
            switch (direction)
            {
                case SwarmDirection.Forward:
                    angle = angle + step;
                    if (angle >= 360)
                    {
                        angle = angle - 360;
                    }

                    break;
                case SwarmDirection.Backward:
                    angle = angle - step;
                    // { sic: tests 360 instead of 0, so the angle is never wrapped back }
                    if (angle < 360)
                    {
                        angle = angle + 360;
                    }

                    break;
            }

            total = total + step;
        }
        while (!(total >= 360));
    }

    internal static void MakeLightningPath(int x1, int y1, int x2, int y2, ref Point3DList list)
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
        length = Round(Math.Sqrt((x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1)));
        dist = 0;
        seg = Random(15) + 5;
        while (dist + seg < length)
        {
            if ((double)a2 > angle)
            {
                a2 = (float)((double)angle - DegToRad(Random(40) + 40));
            }
            else
            {
                a2 = (float)((double)angle + DegToRad(Random(40) + 40));
            }

            var cos = Math.Cos(angle);
            var mathCos = Math.Cos(a2);
            hexX = x1 - Round((double)cos * dist) - Round((double)mathCos * seg);
            var sin = Math.Sin(angle);
            var mathSin = Math.Sin(a2);
            hexY = y1 - Round((double)sin * dist) - Round((double)mathSin * seg);
            MakeLinePath(cx, cy, hexX, hexY, seg, ref list);
            cx = hexX;
            cy = hexY;
            seg = Math.Abs(Round((double)Math.Cos((double)a2 - angle) * seg));
            dist = dist + seg;
            seg = Random(15) + 5;
        }

        MakeLinePath(cx, cy, x2, y2, length - dist, ref list);
        var count = list.Count;
        list[count - 1].Z = 1;
    }

    internal static void MakeArcPath(int X, int Y, int angle, ref float Z, ref float speed, ref float vz, ref float gravity, ref Point3DList list)
    {
        float effect;
        float fy;
        float deltaX;
        float deltaY;
        Point3D xYZ;
        effect = X;
        fy = Y;
        deltaX = (float)((double)Math.Cos(angle) * speed);
        deltaY = (float)((double)Math.Sin(angle) * speed);
        if ((double)gravity > 0)
        {
            while ((double)Z >= 0)
            {
                xYZ = new Point3D();
                xYZ.X = Round(effect);
                xYZ.Y = Round(fy);
                xYZ.Z = Round(Z);
                list.Add(xYZ);
                effect = (float)((double)effect + deltaX);
                fy = (float)((double)fy + deltaY);
                vz = (float)((double)vz - gravity);
                Z = (float)((double)Z + vz);
            }
        }
    }

    internal static void MakeArcPathXY(short x1, short y1, short x2, short y2, short stepSize, short height, ref Point3DList list)
    {
        int I;
        int steps;
        int deltaX;
        int deltaY;
        Point3D xYZ;
        I = 0;
        deltaX = Math.Abs(x2 - x1);
        deltaY = Math.Abs(y2 - y1);
        steps = (deltaX + deltaY) / stepSize;
        if (steps < 2)
        {
            steps = 2;
        }

        while (I <= steps)
        {
            xYZ = new Point3D();
            list.Add(xYZ);
            xYZ.X = x1 + (x2 - x1) * I / steps;
            xYZ.Y = y1 + (y2 - y1) * I / steps;
            var pi = Math.PI;
            xYZ.Z = Round((double)Math.Sin((double)((double)I * pi) / steps) * height);
            I += 1;
        }
    }

    internal static float GetAngle(int x1, int y1, int x2, int y2)
    {
        float result;
        if (x1 == x2)
        {
            result = (float)((double)Math.PI / 2);
        }
        else
        {
            result = (float)Math.Atan((double)(y2 - y1) / (x2 - x1));
        }

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

    internal static void CreateFire(bool spread, int chance, int hexX, int hexY)
    {
        CombatFire fire;
        bool canBurn;
        CombatImageLibraryTerrain terrain;
        CityWall wall;
        if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).TerrainType != 15)
        {
            if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild(2229578) == null)
            {
                if (AoWHSMap.Current.Random(100) < chance)
                {
                    canBurn = false;
                    if (CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).GetDecorationType() == 9 || CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).GetDecorationType() == 1 || CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).GetDecorationType() == 2 || CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).GetDecorationType() == 3)
                    {
                        canBurn = true;
                    }
                    else
                    {
                        terrain = (CombatImageLibraryTerrain)CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild(2228534);
                        if (terrain != null)
                        {
                            if (terrain.GetBurnValue() > 0)
                            {
                                canBurn = true;
                            }
                        }

                        wall = (CityWall)CombatMap.Current.MapContainer.GetMapLevel(0).GetField(hexX, hexY).FindOwnedChild(2229508);
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
                        fire = new CombatFire(null);
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
